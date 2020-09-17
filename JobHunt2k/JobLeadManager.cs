using JobHunt2k.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JobHunt2k
{
    public class JobLeadManager
    {
        private List<JobLead> jobLeads;

        public delegate void UpdateListDisplayCallback(IEnumerable<JobLead> leads);
        private UpdateListDisplayCallback uiCallback;

        public int GetNumberOfJobLeads => jobLeads.Count;

        public JobLeadManager(UpdateListDisplayCallback _uiCallback)
        {
            if (File.Exists("./data"))
                LoadJobLeads();
            else
                jobLeads = new List<JobLead>();

            uiCallback = _uiCallback;
            uiCallback(jobLeads);
        }

        public void AddJobLead(JobLead newJobLead)
        {
            jobLeads.Add(newJobLead);
            uiCallback(jobLeads);
            SaveJobLeads();
        }

        public void RemoveJobLead(JobLead existingJobLead)
        {
            if (jobLeads.Contains(existingJobLead))
            {
                jobLeads.Remove(existingJobLead);
                uiCallback(jobLeads);
                SaveJobLeads();
            }
            else
            {
                // add logging and/or throw exception here...
            }
        }

        public void UpdateJobLead(Guid jobLeadId, JobInfo newJobInfo)
        {
            var foundJobLead = jobLeads
                .Where(jobLead => jobLead.id == jobLeadId)
                .FirstOrDefault();
            if(foundJobLead != null)
            {
                foundJobLead.JobInfo = newJobInfo;
                uiCallback(jobLeads);
                SaveJobLeads();
            }
            else
            {
                // add logging and/or throw exception here...
            }
        }

        public void IncreaseJobLeadDisplayOrder(JobLead leadToIncrease)
        {
            var sortedLeads = jobLeads.OrderBy(lead => lead.DisplayOrder).ToArray();
            for (int i = 0; i < sortedLeads.Length; i++)
            {
                var leadAtIndex = sortedLeads[i];
                if(leadAtIndex == leadToIncrease && i + 1 < sortedLeads.Length)
                {
                    var leadToDecrease = sortedLeads[i + 1];
                    leadToDecrease.DisplayOrder--;
                    leadToIncrease.DisplayOrder++;
                    SaveJobLeads();
                    uiCallback(jobLeads);
                }   
            }
        }

        public void DecreaseJobLeadDisplayOrder(JobLead leadToDecrease)
        {
            var sortedLeads = jobLeads.OrderBy(lead => lead.DisplayOrder).ToArray();
            for (int i = 0; i < sortedLeads.Length; i++)
            {
                var leadAtIndex = sortedLeads[i];
                if (leadAtIndex == leadToDecrease && i > 0)
                {
                    var leadToIncrease = sortedLeads[i - 1];
                    leadToDecrease.DisplayOrder--;
                    leadToIncrease.DisplayOrder++;
                    SaveJobLeads();
                    uiCallback(jobLeads);
                }
            }
        }

        public void SaveJobLeads()
        {
            var leadsJosn = JsonConvert.SerializeObject(jobLeads);
            File.WriteAllText("./data", leadsJosn);
        }

        public void LoadJobLeads()
        {
            var leadsJosn = File.ReadAllText("./data");
            jobLeads = JsonConvert.DeserializeObject<List<JobLead>>(leadsJosn);
        }
    }
}

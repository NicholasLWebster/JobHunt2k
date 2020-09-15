using System;

namespace JobHunt2k.Models
{
    public class JobLead
    {
        public Guid id { get; set; }
        public JobInfo JobInfo { get; set; }

        public JobLead(Guid _id,JobInfo _jobInfo)
        {
            id = _id;
            JobInfo = _jobInfo;
        }

        public override string ToString()
        {
            return  JobInfo.IsFavorite ? $"⭐{JobInfo.DisplayTitle}⭐" : JobInfo.DisplayTitle;
        }
    }
}

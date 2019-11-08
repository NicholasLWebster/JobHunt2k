using JobHunt2k.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace JobHunt2k
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private JobLeadManager leadManager;
        private JobLead selectedJobLead;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            leadManager = new JobLeadManager(UpdateDisplayList);
        }

        private void UpdateDisplayList(IEnumerable<JobLead> leads)
        {
            JobLeads_ListBox.Items.Clear();
            Edit_Button.IsEnabled = false;
            Delete_Button.IsEnabled = false;
            selectedJobLead = null;
            leads.Select(JobLeads_ListBox.Items.Add).ToList(); // ToList() because otherwise it won't actually iterate over.
        }

        private void CreateJobLead(Guid jobListingId, JobInfo jobInfo)
        {
            var newLead = new JobLead(jobListingId, jobInfo);
            leadManager.AddJobLead(newLead);
        }

        private void UpdateJobLead(Guid jobListingId, JobInfo jobInfo)
        {
            leadManager.UpdateJobLead(jobListingId, jobInfo);
        }

        private void Exit_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CreateNew_Button_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new EditJobListingWindow(Guid.NewGuid(), new JobInfo(), CreateJobLead);
            editWindow.Show();
        }

        private void JobLeads_ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedJobLead = (JobLead)JobLeads_ListBox.SelectedItem;
            Edit_Button.IsEnabled = true;
            Delete_Button.IsEnabled = true;
        }

        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new EditJobListingWindow(selectedJobLead.id, selectedJobLead.JobInfo, UpdateJobLead);
            editWindow.Show();
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Delete Job Lead?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                leadManager.RemoveJobLead(selectedJobLead);
                selectedJobLead = null;
            }
        }
    }
}

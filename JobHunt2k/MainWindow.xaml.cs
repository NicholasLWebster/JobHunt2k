using JobHunt2k.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            var leadsList = leads
                .ToList()
                .OrderBy(lead => lead.DisplayOrder);
            foreach (var lead in leadsList)
            {
                var listBoxItem = new ListBoxItem
                {
                    Content = lead,
                    FontWeight = lead.JobInfo.IsFavorite ? FontWeights.Bold : FontWeights.Normal,
                    Foreground = lead.JobInfo.IsFavorite ? Brushes.DarkGoldenrod : Brushes.Black,
                };
                JobLeads_ListBox.Items.Add(listBoxItem);
            }
        }

        private void CreateJobLead(Guid jobListingId, JobInfo jobInfo)
        {
            var newLead = new JobLead(jobListingId, jobInfo);
            newLead.DisplayOrder = leadManager.GetNumberOfJobLeads;
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
            if (JobLeads_ListBox.SelectedItem != null)
            {
                var listBoxItem = (ListBoxItem)JobLeads_ListBox.SelectedItem;
                selectedJobLead = (JobLead)listBoxItem.Content;
                Edit_Button.IsEnabled = true;
                Delete_Button.IsEnabled = true;
                IncreaseDisplayOrder_Button.IsEnabled = JobLeads_ListBox.SelectedIndex < JobLeads_ListBox.Items.Count - 1;
                DecreaseDisplayOrder_Button.IsEnabled = JobLeads_ListBox.SelectedIndex > 0;
            }
            else
            {
                Edit_Button.IsEnabled = false;
                Delete_Button.IsEnabled = false;
                IncreaseDisplayOrder_Button.IsEnabled = false;
                DecreaseDisplayOrder_Button.IsEnabled = false;
            }
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

        private void IncreaseDisplayOrder_Button_Click(object sender, RoutedEventArgs e)
        {
            var currentSelectedLeadIndex = JobLeads_ListBox.SelectedIndex; 
            leadManager.IncreaseJobLeadDisplayOrder(selectedJobLead);
            JobLeads_ListBox.SelectedItem = JobLeads_ListBox.Items[currentSelectedLeadIndex + 1];
            
        }

        private void DecreaseDisplayOrder_Button_Click(object sender, RoutedEventArgs e)
        {
            var currentSelectedLeadIndex = JobLeads_ListBox.SelectedIndex;
            leadManager.DecreaseJobLeadDisplayOrder(selectedJobLead);
            JobLeads_ListBox.SelectedItem = JobLeads_ListBox.Items[currentSelectedLeadIndex - 1];
        }
    }
}

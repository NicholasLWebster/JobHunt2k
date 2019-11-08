using JobHunt2k.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JobHunt2k
{
    /// <summary>
    /// Interaction logic for EditJobListingWindow.xaml
    /// </summary>
    public partial class EditJobListingWindow : Window
    {
        private Guid jobListingId;
        private JobInfo jobInfo;

        public delegate void SaveCallBack(Guid jobListingId, JobInfo jobInfo);
        private SaveCallBack saveCallBack;


        public EditJobListingWindow(Guid _jobListingId, JobInfo _jobInfo, SaveCallBack _saveCallBack)
        {
            jobListingId = _jobListingId;
            jobInfo = _jobInfo;
            saveCallBack = _saveCallBack;
            InitializeComponent();
            Loaded += EditJobListingWindow_Loaded;
        }

        public void EditJobListingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateInputValues(jobListingId, jobInfo);
        }

        private void UpdateInputValues(Guid jobId, JobInfo jobInfo)
        {
            JobLeadId_TextBox.Text = jobId.ToString();
            DisplayTitle_TextBox.Text = jobInfo.DisplayTitle;
            Source_TextBox.Text = jobInfo.Source;
            CompanyName_TextBox.Text = jobInfo.CompanyName;
            Position_TextBox.Text = jobInfo.JobPosition;
            Salary_TextBox.Text = jobInfo.Salary;
            Recruiter_TextBox.Text = jobInfo.Recruiter;
            ContactInfo_TextBox.Text = jobInfo.ContactInfo;
            Notes_TextBox.Text = jobInfo.JobNotes;
        }

        private JobInfo GetInputValues()
        {
            return new JobInfo
            {
                DisplayTitle = DisplayTitle_TextBox.Text,
                Source = Source_TextBox.Text,
                CompanyName = CompanyName_TextBox.Text,
                JobPosition = Position_TextBox.Text,
                Salary = Salary_TextBox.Text,
                Recruiter = Recruiter_TextBox.Text,
                ContactInfo = ContactInfo_TextBox.Text,
                JobNotes = Notes_TextBox.Text
            };
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            var currentInput = GetInputValues();
            saveCallBack(jobListingId, currentInput);
            Close();
        }
    }
}

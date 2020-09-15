namespace JobHunt2k.Models
{
    public class JobInfo
    {
        public string DisplayTitle { get; set; }
        public string Source { get; set; }
        public string CompanyName { get; set; }
        public string JobPosition { get; set; }
        public string Salary { get; set; }
        public string Recruiter { get; set; }
        public string ContactInfo { get; set; }
        public string JobNotes { get; set; }
        public bool IsFavorite { get; set; }
    }
}

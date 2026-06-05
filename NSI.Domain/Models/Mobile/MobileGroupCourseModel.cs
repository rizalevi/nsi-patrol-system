namespace NSI.Domain.Models.Mobile
{
    public class MobileGroupCourseModel
    {
        public string Group_ID { get; set; }
        public string Course_Code { get; set; }
        public string Plant_Code { get; set; }
        public string Allow_Show { get; set; }
        public DateTime? Register_Date { get; set; } 
        public string Register_Program { get; set; }
        public string Register_User { get; set; }
        public DateTime? Update_Date { get; set; }
        public string Update_Program { get; set; }
        public string Update_User { get; set; }
    }
} 
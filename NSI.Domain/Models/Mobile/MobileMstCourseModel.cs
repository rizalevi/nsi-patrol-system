namespace NSI.Domain.Models.Mobile
{
    public class MobileMstCourseModel
    {
        public string Course_Code { get; set; }
        public string Title { get; set; }
        public string Doc_No { get; set; }
        public string Plant_Code { get; set; }
        public string Check_Type { get; set; }
        public string Shift_Cls { get; set; }
        public string Register_User { get; set; }
        public DateTime? Register_Date { get; set; }
        public string Last_User { get; set; }
        public DateTime? Last_Update { get; set; }
    }
} 
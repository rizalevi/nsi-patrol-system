namespace NSI.Domain.Models.Mobile
{
    public class MobileInspectionPointModel
    {
        public string Course_Code { get; set; }
        public string Group_Code { get; set; }
        public string Inspection_Code { get; set; }
        public string Inspection_Point { get; set; }

        private string _unitCode;
        public string Unit_Code
        {
            get => _unitCode;
            set => _unitCode = value?.Trim();
        }
        public string Min { get; set; }
        public string Max { get; set; }
        public string Option_1 { get; set; }
        public string Option_2 { get; set; }
        public string Chart { get; set; }
        public DateTime? Register_Date { get; set; }
        public string Register_User { get; set; }
        public DateTime? Last_Update { get; set; }
        public string Last_User { get; set; }
    }
} 
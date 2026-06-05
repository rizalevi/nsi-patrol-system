namespace NSI.Domain.Models.Mobile
{
    public class MobileMstUnitModel
    {
        private string _unitCode;
        public string Unit_Code
        {
            get => _unitCode;
            set => _unitCode = value?.Trim();
        }
        public string Methode { get; set; }
        public string Methode_Description { get; set; }
        public string Unit_Description { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public DateTime? Register_Date { get; set; }
        public string Register_User { get; set; }
        public DateTime? Last_Update { get; set; }
        public string Last_User { get; set; }
    }
}

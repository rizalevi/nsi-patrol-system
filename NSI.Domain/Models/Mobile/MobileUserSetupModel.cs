namespace NSI.Domain.Models.Mobile
{
    public class MobileUserSetupModel
    {
        private string _userID;
        public string User_ID
        {
            get => _userID;
            set => _userID = value?.Trim();
        }

        private string _userName; 
        public string User_Name
        {
            get => _userName;
            set => _userName = value?.Trim();
        }

        private string _password;

        public string Password
        {
            get => _password;
            set => _password = value?.Trim();
        }

        public string Status_Admin { get; set; }
        public string Employee_ID { get; set; }
        public string Group_Code { get; set; }
        public DateTime? Password_Expired { get; set; }
        public DateTime? Last_Update { get; set; }
    }
}

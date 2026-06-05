
namespace NSI.Domain.Models.Security
{
    public class ChangePassword
    {
        public string? UserID { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
        public DateTime? PasswordExpired { get; set; }
       
    }


}

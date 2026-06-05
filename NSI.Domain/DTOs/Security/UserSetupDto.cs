using System.Text.Json.Serialization;

namespace NSI.Domain.DTOs.Security
{
    public class UserSetupDto
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string? UserGroupName { get; set; }
        public string? Email {  get; set; }
        public string? StatusEmail { get; set; }
        public string? ClusterName { get; set; }
        public string? AdminStatus { get; set; }
        public string? LockedStatus { get; set; }
        public DateTime PasswordExpired { get; set; }
        public DateTime RegisterDate { get; set; }
        public string? RegisterUser { get; set; }
        public DateTime LastUpdate { get; set; }
        public string? LastUser { get; set; }

    }

    public class UserSettingCourseDto
    {
        public string? UserID { get; set; }
        public string? UserName { get; set; }
        public string? PlantCode { get; set; }
        public string? PlantName { get; set; }
        public string? CourseCode { get; set; }
        public string? Title { get; set; }
        public bool AllowAccess { get; set; }
    }

    public class ProfileDto
    {
        public string? UserID { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? UserGroupCode { get; set; }
        public string? UserGroupName { get; set; }
        public string? Email { get; set; }
        public DateTime? PasswordExpired { get; set; }
        public string? PictureName { get; set; }
        public string? PicturePath { get; set; }
        //public HttpPostedFileWrapper PicturePath { get; set; }
    }
}

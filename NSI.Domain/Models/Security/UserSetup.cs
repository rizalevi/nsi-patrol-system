
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace NSI.Domain.Models.Security
{
    public class UserSetup //savedata
    {
        public string UserID { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? UserGroup { get; set; } = null;
        public string? Email {  get; set; } 
        public string? StatusEmail { get; set; }
        public string? StatusAdmin { get; set; }
        public string? Locked { get; set; }
        public DateTime? PasswordExpired { get; set; }
        public string? PictureName { get; set; }
        public string? PicturePath { get; set; }
        [JsonIgnore]
        public IFormFile? PictureFile { get; set; }

        public byte[]? FileImageBytes { get; set; }

        //password for login desktop / HT
        public string? PasswordOld { get; set; }
    }

    public class UserSetupInfo
    {
        public string UserID { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? UserGroupCode { get; set; }
        public string? UserGroupName { get; set; }
        public string? Email { get; set; }
        public string? StatusEmail { get; set; }
        public string? ClusterCode { get; set; }
        public string? ClusterName { get; set; }
        public string? StatusAdmin { get; set; }
        public string? Locked { get; set; }
        public DateTime? PasswordExpired { get; set; }

    }

    public class UserApprovalInfo
    {
        public bool IsInspector { get; set; }
        public bool IsChecker { get; set; }
        public bool IsLeader { get; set; }
        public bool IsSupervisor { get; set; }
    }

    public class UserSettingCourse
    {
        public string? UserID { get; set; }
        public string? PlantCode { get; set; }
        public string? CourseCode { get; set; }
        public bool AllowAccess { get; set; }
    }

}

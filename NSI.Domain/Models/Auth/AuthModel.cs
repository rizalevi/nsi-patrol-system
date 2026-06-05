using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.Models.Auth
{
    public class AuthModel : Login
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public DateTime ExpiredAt { get; set; }
        public string Role { get; set; } = "";
        public string Email { get; set; } = "";
        public string? UserName { get; set; } = "";
        public string StatusAdmin { get; set; } = "";
        public string Locked { get; set; } = "";
        public string GroupUser { get; set; } = "";
        public string? PicturePath { get; set; }
    }
}

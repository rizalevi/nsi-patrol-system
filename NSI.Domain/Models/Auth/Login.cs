

namespace NSI.Domain.Models.Auth
{
    public  class Login
    {
        public string UserId { get; set; }
        public string? UserName { get; set; }
        public string Password { get; set; }
        //public string? Email { get; set; }  
        //public string? PicturePath { get; set; }
        public string? ErrorMessage { get; set; }
        public string? IPAddress { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime ExpiredAt { get; set; }
    }

    

}

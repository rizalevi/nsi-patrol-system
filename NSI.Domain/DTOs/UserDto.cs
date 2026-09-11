
namespace NSI.Domain.DTOs
{
    public  class UserDto
    {
        public string UserId { get; set; } = default!;
        public string Password { get; set; } = default!;
    }


    public class AuthUser
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PicturePath { get; set; }
        public string? Password { get; set; }
        public string? StatusAdmin { get; set; }
        public DateTime ExpiredPassword { get; set; }
    }


}

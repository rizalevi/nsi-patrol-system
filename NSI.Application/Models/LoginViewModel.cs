using System.ComponentModel.DataAnnotations;

namespace NSI.Application.Models
{
    public class LoginViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? ErrorMessage { get; set; }
    }
}

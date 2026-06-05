using System.ComponentModel.DataAnnotations;

namespace NSI.Domain.Models.Auth
{
    public  class ChangePasswordRequest
    {
        [Required]
        public string OldPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(NewPassword),
            ErrorMessage = "Confirm password does not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

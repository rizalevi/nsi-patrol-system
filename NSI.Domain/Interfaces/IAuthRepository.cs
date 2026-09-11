using NSI.Domain.DTOs;
using NSI.Domain.DTOs.Email;
using NSI.Domain.Models.Auth;

namespace NSI.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task<AuthModel?> GetByUserIdAsync(string userId);
        //Task<string?> GetEncryptedPasswordAsync(string userId);
        
        Task<bool> ChangePasswordAsync(
        string userId,
        string oldPassword,
        string newPassword);

        Task<DateTime?> CheckExpiredPassword(string userId);

        //email login auth
        Task<EmailLoginInfoDto> GetByTokenAsync(Guid token,string type);

        Task<bool?> MarkEmailLinkVisitedAsync(Guid token);
    }
}

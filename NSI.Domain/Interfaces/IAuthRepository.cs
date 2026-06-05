using NSI.Domain.DTOs;
using NSI.Domain.Models.Auth;

namespace NSI.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task<AuthModel?> GetByUserIdAsync(string userId);
        Task<string?> GetEncryptedPasswordAsync(string userId);
        //Task<UserDto?> GetEncryptedPasswordAsync(string userId);
        //Task<AuthModel> GetUserDataFromDBByIDAndPassword(Login param);
        Task<bool> ChangePasswordAsync(
        string userId,
        string oldPassword,
        string newPassword);

        Task<DateTime?> CheckExpiredPassword(string userId);
    }
}

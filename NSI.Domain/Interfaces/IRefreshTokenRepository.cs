
using NSI.Domain.Auth;

namespace NSI.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task SaveAsync(RefreshToken token);
        Task<RefreshToken?> GetByHashAsync(string hash);
        Task RevokeAsync(Guid id);
        Task RevokeAllAsync(string userId);
    }
}

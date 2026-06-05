using NSI.Domain.Auth;

namespace NSI.Domain.Services
{
    public interface IRefreshTokenService
    {
        Task<string> CreateAsync(string userId);
        Task<RefreshToken?> ValidateAsync(string rawToken);
        Task RotateAsync(RefreshToken token);
        Task RevokeAsync(string rawToken);
    }
}

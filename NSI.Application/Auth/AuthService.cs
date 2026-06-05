using NSI.Domain.Auth;
using NSI.Domain.Interfaces;
using NSI.Infra.Services;

namespace NSI.Application.Auth
{
    public class AuthService
    {
        private readonly JwtService _jwt;
        private readonly IHashService _hash;
        private readonly IRefreshTokenRepository _repo;

        public AuthService(
            JwtService jwt,
            IHashService hash,
            IRefreshTokenRepository repo)
        {
            _jwt = jwt;
            _hash = hash;
            _repo = repo;
        }

        public async Task<(string accessToken, string refreshToken)> LoginAsync(
            string userId,
            string role)
        {
            var accessToken = _jwt.GenerateToken(userId, out DateTime expiredAt);

            var refreshToken = Guid.NewGuid().ToString();
            var hash = _hash.Hash(refreshToken);

            var entity = new RefreshToken(
                userId,
                hash,
                DateTime.UtcNow.AddDays(30)
            );

            await _repo.SaveAsync(entity);

            return (accessToken, refreshToken);
        }

        public async Task<(string accessToken, string refreshToken)> RefreshAsync(
            string refreshToken)
        {
            var hash = _hash.Hash(refreshToken);
            var stored = await _repo.GetByHashAsync(hash);

            if (stored == null || stored.IsExpired() || stored.IsRevoked)
                throw new UnauthorizedAccessException();

            await _repo.RevokeAsync(stored.Id);

            var newRefreshToken = Guid.NewGuid().ToString();
            var newHash = _hash.Hash(newRefreshToken);

            var newEntity = new RefreshToken(
                stored.UserId,
                newHash,
                DateTime.UtcNow.AddDays(30)
            );

            await _repo.SaveAsync(newEntity);

            var newAccessToken = _jwt.GenerateToken(
                stored.UserId, out DateTime expiredAt
            );

            return (newAccessToken, newRefreshToken);
        }
    }
}

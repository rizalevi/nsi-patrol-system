using NSI.Domain.Auth;
using NSI.Domain.Interfaces;
using System.Security.Cryptography;

namespace NSI.Api.Auth
{
    public class RefreshTokenService
    {
        private readonly IRefreshTokenRepository _repo;
        private readonly IHashService _hashService;

        public RefreshTokenService(
            IRefreshTokenRepository repo,
            IHashService hashService)
        {
            _repo = repo;
            _hashService = hashService;
        }

        public async Task<string> CreateAsync(string userId)
        {
            var raw = Convert.ToBase64String(
                RandomNumberGenerator.GetBytes(64));

            var hash = _hashService.Hash(raw);

            var token = new RefreshToken(
                userId,
                hash,
                DateTime.UtcNow.AddDays(7)
            );

            await _repo.SaveAsync(token);
            return raw;
        }

        public async Task<RefreshToken?> ValidateAsync(string rawToken)
        {
            var hash = _hashService.Hash(rawToken);
            var token = await _repo.GetByHashAsync(hash);

            if (token == null || token.IsRevoked || token.IsExpired())
                return null;

            return token;
        }

        public async Task RotateAsync(RefreshToken token)
        {
            await _repo.RevokeAsync(token.Id);
        }

        public async Task RevokeAsync(string rawToken)
        {
            var hash = _hashService.Hash(rawToken);
            var token = await _repo.GetByHashAsync(hash);

            if (token != null)
                await _repo.RevokeAsync(token.Id);
        }

        public async Task RevokeAllAsync(string userId)
        {
            await _repo.RevokeAllAsync(userId);
        }

    }
}

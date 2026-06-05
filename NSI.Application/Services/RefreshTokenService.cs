using NSI.Domain.Auth;
using NSI.Domain.Interfaces;
using NSI.Domain.Services;
using System.Security.Cryptography;
using System.Text;

namespace NSI.Application.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _repo;
        private readonly IHashService _hashService;

       

        public RefreshTokenService(IRefreshTokenRepository repo, IHashService hashService)
        {
            _repo = repo;
            _hashService = hashService;
        }

        public async Task<string> CreateAsync(string userId)
        {
            var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var hash = Hash(rawToken);

            var token = new RefreshToken(
                userId,
                hash,
                DateTime.UtcNow.AddDays(7)
            );

            await _repo.SaveAsync(token);
            return rawToken;
        }

        public async Task<RefreshToken?> ValidateAsync(string rawToken)
        {
            var hash = Hash(rawToken);
            var token = await _repo.GetByHashAsync(hash);

            if (token == null) return null;
            if (token.IsRevoked) return null;
            if (token.IsExpired()) return null;

            return token;
        }

        public async Task RotateAsync(RefreshToken token)
        {
            token.Revoke();
            await _repo.RevokeAsync(token.Id);
        }

        private static string Hash(string input)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(
                sha.ComputeHash(Encoding.UTF8.GetBytes(input))
            );
        }

        public async Task RevokeAsync(string rawToken)
        {
            var hash = _hashService.Hash(rawToken);
            var token = await _repo.GetByHashAsync(hash);

            if (token != null)
                //await _repo.RevokeAsync(token.Id);
                await _repo.RevokeAllAsync(token.UserId);
        }
    }

}

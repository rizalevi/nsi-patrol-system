using Dapper;
using NSI.Domain.Auth;
using NSI.Domain.Interfaces;
using NSI.Infra.Datas;
using System.Data.Common;

namespace NSI.Infra.Repositories 
{
    public  class RefreshTokenRepository : IRefreshTokenRepository
    {

        private readonly IDbConnectionFactory _connectionFactory;
        public RefreshTokenRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        public async Task SaveAsync(RefreshToken token)
        {
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync("""
            INSERT INTO RefreshTokens
            VALUES (@Id, @UserId, @TokenHash, @ExpiresAt, @IsRevoked, GETUTCDATE())
        """, token);
        }

        public async Task<RefreshToken?> GetByHashAsync(string hash)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<RefreshToken>(
                "SELECT * FROM RefreshTokens WHERE TokenHash=@hash",
                new { hash });
        }

        public async Task RevokeAsync(Guid id)
        {
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync(
                "UPDATE RefreshTokens SET IsRevoked=1 WHERE Id=@id",
                new { id });
        }

        public async Task RevokeAllAsync(string userId)
        {
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync(
                "UPDATE RefreshTokens SET IsRevoked=1 WHERE UserId=@userId",
                new { userId });
        }
    }
}

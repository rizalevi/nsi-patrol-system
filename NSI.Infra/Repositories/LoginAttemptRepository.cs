using Dapper;
using Microsoft.AspNetCore.Connections;
using NSI.Domain.Auth;
using NSI.Domain.Interfaces;
using NSI.Domain.Models.Auth;
using NSI.Infra.Datas;
using System.Data;

namespace NSI.Infra.Repositories
{
    public class LoginAttemptRepository : ILoginAttemptRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public LoginAttemptRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<LoginAttempt?> GetByIpAsync(string ip, string userId)
        {
            using var _db = _connectionFactory.CreateConnection();
            return await _db.QuerySingleOrDefaultAsync<LoginAttempt>(
                "sp_LoginAttempt_Get",
                new
                {
                    IpAddress = ip,
                    UserId = userId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task SaveAsync(LoginAttempt attempt)
        {
            using var _db = _connectionFactory.CreateConnection();
            await _db.ExecuteAsync(
                "sp_LoginAttempt_Insert",
                new
                {
                    attempt.Id,
                    attempt.IpAddress,
                    attempt.UserId,
                    attempt.FailedCount,
                    attempt.LockedUntil,
                    attempt.LastAttemptAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateAsync(LoginAttempt attempt)
        {
            using var _db = _connectionFactory.CreateConnection();
            await _db.ExecuteAsync(
                "sp_LoginAttempt_Update",
                new
                {
                    attempt.IpAddress,
                    attempt.UserId,
                    attempt.FailedCount,
                    attempt.LockedUntil,
                    attempt.LastAttemptAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task ResetAsync(string ip, string userId)
        {
            using var _db = _connectionFactory.CreateConnection();
            await _db.ExecuteAsync(
                "sp_LoginAttempt_Reset",
                new
                {
                    IpAddress = ip,
                    UserId = userId
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}

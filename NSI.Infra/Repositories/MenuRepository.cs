using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NSI.Domain.DTOs;
using NSI.Domain.Interfaces;
using NSI.Infra.Datas;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace NSI.Infra.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHashService _hashService;

        public MenuRepository(
           IDbConnectionFactory connectionFactory,
           IHashService hashService)
        {
            _connectionFactory = connectionFactory;
            _hashService = hashService;
        }

        public async Task<List<UserMenuDto>> GetMenusAsync(string userId)
        {
            using var conn = _connectionFactory.CreateConnection();

            var result = await conn.QueryAsync<UserMenuDto>(
                "sp_SS_UserMenu",
                new
                {
                    UserId = userId
                },
                commandType: CommandType.StoredProcedure
            );

            return result.AsList();
        }

       
    }
}

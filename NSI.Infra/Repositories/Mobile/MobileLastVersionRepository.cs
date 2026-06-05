using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NSI.Domain.DTOs.Mobile;
using NSI.Domain.Interfaces.Mobile; 
using System.Data; 

namespace NSI.Infra.Repositories.Mobile
{
    public class MobileLastVersionRepository : IMobileLastVersionRepository
    {
        private readonly string _connString;
        public MobileLastVersionRepository(IConfiguration config)
        {
            _connString = config.GetConnectionString("DBCS");
        }
        public async Task<MobileLastVersionDto?> GetVersion()
        {
            using var conn = new SqlConnection(_connString);

            var result = await conn.QueryFirstOrDefaultAsync<MobileLastVersionDto>(
                 "select * from vw_GetLastVersionMobile" 
            );  

            return result;

        }

    }
}

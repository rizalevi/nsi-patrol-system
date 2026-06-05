using NSI.Domain.DTOs.Logger;
using NSI.Domain.Interfaces.Logger;
using NSI.Domain.Models.Logger;
using NSI.Infra.Datas;


namespace NSI.Infra.Repositories.Logger
{
    public class ErrorLogRepository : IErrorLogRepository
    {
        private readonly DbExecutor _dbExecutor;

        public ErrorLogRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task LogAsync(ErrorLogModel log)
        {
            var sql = @"
            INSERT INTO ErrorLogs 
            (Message, StackTrace, Path, Method, UserId, QueryString, RequestId)
            VALUES 
            (@Message, @StackTrace, @Path, @Method, @UserId, @QueryString, @RequestId)";

            await _dbExecutor.ExecuteAsync(sql, log, System.Data.CommandType.Text);
        }

        public async Task<IEnumerable<ErrorDto>> GetAll()
        {
            var sql = @"sp_WEB_GetError_List";
            return await _dbExecutor.QueryListAsync<ErrorDto>(sql, System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ErrorDto>> Search(string keyword)
        {
            var sql = @"
            SELECT TOP 100 *
            FROM ErrorLogs
            WHERE Message LIKE @Keyword
               OR Path LIKE @Keyword
               OR UserId LIKE @Keyword
            ORDER BY CreatedAt DESC";

            return await _dbExecutor.QueryListAsync<ErrorDto>(sql, new { Keyword = "%" + keyword + "%" }, System.Data.CommandType.Text);
        }
    }
}

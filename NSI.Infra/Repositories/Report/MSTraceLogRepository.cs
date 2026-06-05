using NSI.Domain.DTOs.Report;
using NSI.Domain.Interfaces.Report;
using NSI.Infra.Datas;

namespace NSI.Infra.Repositories.Report
{
    public class MSTraceLogRepository(DbExecutor dbExecutor) : IMSTraceLogRepository
    {
        public async Task<IEnumerable<MSTraceLogDto>> GetAllAsync(string Module = "", string LogDateFrom = "", string LogDateTo = "") => await dbExecutor.QueryListAsync<MSTraceLogDto>("sp_Web_TraceLog_List", new { Module, LogDateFrom, LogDateTo });

        public async Task<IEnumerable<MSTraceLogComboDto>> GetComboAsync() => await dbExecutor.QueryListAsync<MSTraceLogComboDto>("sp_Web_TraceLog_Combo");
    }
}

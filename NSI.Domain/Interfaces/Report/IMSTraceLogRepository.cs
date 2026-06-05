using NSI.Domain.DTOs.Report;
using NSI.Domain.Models.Master;

namespace NSI.Domain.Interfaces.Report
{
    public interface IMSTraceLogRepository
    {
        Task<IEnumerable<MSTraceLogDto>> GetAllAsync(string Module = "", string LogDateFrom = "", string LogDateTo = "");
        Task<IEnumerable<MSTraceLogComboDto>> GetComboAsync();
    }
}
using NSI.Domain.DTOs.Dashboard;

namespace NSI.Domain.Interfaces.Dashboard
{
    public interface IDashboardRepository
    {
        Task<DashboardDto> GetDashboardOverview(DashboardParamDto model);

        Task<List<ProgressInspectionDetailDto>> ListRemainingData(ViewDetailProgressParamDto model); //remain & uncompleted today or data until today

        Task<List<RealtimeShiftDetailAbnormalDto>> ListAbnormalRealtimeShift(DashboardParamDto model);
    }
}

using Dapper;
using NSI.Domain.DTOs.Dashboard;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.Interfaces.Dashboard;
using NSI.Infra.Datas;
using System.Data;

namespace NSI.Infra.Repositories.Dashboard
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DbExecutor _dbExecutor;

        public DashboardRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }


        public async Task<DashboardDto> GetDashboardOverview(DashboardParamDto model)
        {
            try
            {
                return await _dbExecutor.QueryMultipleAsync(
                    "sp_WEB_Dashboard_GetOverview",
                    new { UserId = model.UserId },
                    async multi =>
                    {
                        var result = new DashboardDto
                        {
                            SummaryProcess = await multi.ReadSingleOrDefaultAsync<SummaryProcessDto>(),
                            SummaryProgressInspection = await multi.ReadSingleOrDefaultAsync<SummaryProgressInspectionDto>(),
                            //ProgressInspectionDetails = (await multi.ReadAsync<ProgressInspectionDetailDto>()).AsList(),
                            TopPlantAbnormal = (await multi.ReadAsync<TopPlantAbnormalDto>()).AsList(),
                            TopEquipmentGroupAbnormal = (await multi.ReadAsync<TopEquipmentGroupAbnormalDto>()).AsList(),
                            RealtimeShifts = (await multi.ReadAsync<RealtimeShiftDto>()).AsList()
                        };

                        return result;
                    });
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get dashboard data: {ex.Message}", ex);
            }
        }
        public async Task<List<ProgressInspectionDetailDto>> ListRemainingData(ViewDetailProgressParamDto model)
        {

            if (model.Type == "remaining")
            {
                
                string sp = "sp_WEB_Dashboard_ProgressInspectionDetail_ListToday";
                var result = await _dbExecutor.QueryListAsync<ProgressInspectionDetailDto>(sp, new { UserID = model.UserId });
                return result.ToList();

            }
            else
            {
                string sp = "sp_WEB_Dashboard_ProgressInspectionDetail_ListAll";
                var result = await _dbExecutor.QueryListAsync<ProgressInspectionDetailDto>(sp, new { UserID = model.UserId });
                return result.ToList();
            }


        }

        public async Task<List<RealtimeShiftDetailAbnormalDto>> ListAbnormalRealtimeShift(DashboardParamDto model)
        {
            string sp = "sp_WEB_Dashboard_RealtimeShift_GetDetailAbnormal";
            var result = await _dbExecutor.QueryListAsync<RealtimeShiftDetailAbnormalDto>(sp, new { UserID = model.UserId });
            return result.ToList();
        }


    }
}
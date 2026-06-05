using NSI.Infra.Datas;
using NSI.Domain.DTOs.InspectionResult;
using NSI.Domain.Interfaces.InspectionResult;

namespace NSI.Infra.Repositories.InspectionResult
{
    public class InspectionResultSummaryByItemRepository : IInspectionResultSummaryByItemRepository
    {
        private readonly DbExecutor _dbExecutor;

        public InspectionResultSummaryByItemRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }


        public async Task<List<UserPlantDto>> GetUserPlant(string UserID)
        {
            string sp = "sp_WEB_GetPlantGroupUser";
            var result = await _dbExecutor.QueryListAsync<UserPlantDto>(sp, new { UserID });
            return result.ToList();
        }

        //datalist & chart
        public async Task<List<InspectionResultSummaryByItemDto>> GetData(InspectionResultByItemParamDto model)
        {

            string sp = "sp_WEB_InspectionResultSummaryByItem_GetData";
            var result = await _dbExecutor.QueryListAsync<InspectionResultSummaryByItemDto>(sp, model);
            return result.ToList();

        }

    }
}

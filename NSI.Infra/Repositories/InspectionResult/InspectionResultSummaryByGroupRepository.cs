using NSI.Infra.Datas;
using NSI.Domain.DTOs.InspectionResult;
using NSI.Domain.Interfaces.InspectionResult;

namespace NSI.Infra.Repositories.InspectionResult
{
    public class InspectionResultSummaryByGroupRepository : IInspectionResultSummaryByGroupRepository
    {
        private readonly DbExecutor _dbExecutor;

        public InspectionResultSummaryByGroupRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }


        //datalist & chart
        public async Task<List<InspectionResultSummaryByGroupDto>> GetData(InspectionResultByGroupParamDto model)
        {

            string sp = "sp_WEB_InspectionResultSummaryByGroup_GetData";
            var result = await _dbExecutor.QueryListAsync<InspectionResultSummaryByGroupDto>(sp, model);
            return result.ToList();

        }

    }
}

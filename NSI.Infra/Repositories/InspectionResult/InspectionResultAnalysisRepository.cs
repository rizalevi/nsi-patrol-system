using NSI.Infra.Datas;
using NSI.Domain.DTOs.InspectionResult;
using NSI.Domain.Interfaces.InspectionResult;

namespace NSI.Infra.Repositories.InspectionResult
{
    public class InspectionResultAnalysisRepository : IInspectionResultAnalysisRepository
    {
        private readonly DbExecutor _dbExecutor;

        public InspectionResultAnalysisRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }


        public async Task<List<UserPlantDto>> GetUserPlant(string UserID)
        {
            string sp = "sp_WEB_GetPlantGroupUser";
            var result = await _dbExecutor.QueryListAsync<UserPlantDto>(sp, new { UserID });
            return result.ToList();
        }

        //datalist
        public async Task<List<InspectionResultAnalysisDto>> GetData(InspectionResultParamDto model)
        {

            string sp = "sp_WEB_InspectionResultAnalysis_GetData";
            var result = await _dbExecutor.QueryListAsync<InspectionResultAnalysisDto>(sp, model);
            return result.ToList();

        }

        public async Task<List<InspectionResultAnalysisChartDto>> GetChart(InspectionResultParamDto model)
        {
            string sp = "sp_WEB_InspectionResultAnalysis_GetChart";
            var result = await _dbExecutor.QueryListAsync<InspectionResultAnalysisChartDto>(sp, model);
            return result.ToList();

        }

        public async Task<List<InspectionGetUnitDto>> GetUnitName(InspectionResultParamDto model)
        {
            string sp = "sp_WEB_InspectionResultAnalysis_GetUnit";
            var result = await _dbExecutor.QueryListAsync<InspectionGetUnitDto>(sp,new { CourseCode = model.CourseCode, InspectionPointCode = model.InspectionPointCode});
            return result.ToList();
        }
    }
}

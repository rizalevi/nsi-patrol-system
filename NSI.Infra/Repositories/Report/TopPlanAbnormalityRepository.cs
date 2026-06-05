using NSI.Domain.DTOs.Report;
using NSI.Domain.Interfaces.Report;
using NSI.Infra.Datas;

namespace NSI.Infra.Repositories.Report
{
    public class TopPlanAbnormalityRepository(DbExecutor dbExecutor) : ITopPlanAbnormalityRepository
    {
        public async Task<IEnumerable<TopPlanAbnormalityPlantDto>> GetDataPlantAsync(TopPlanAbnormalityPlantParamDto model) 
            => await dbExecutor.QueryListAsync<TopPlanAbnormalityPlantDto>("sp_WEB_TopPlanAbnormalityPlant_GetData", model);

        public async Task<IEnumerable<TopPlanAbnormalityPlantDto>> GetChartByPlant(TopPlanAbnormalityPlantParamDto model)
    => await dbExecutor.QueryListAsync<TopPlanAbnormalityPlantDto>("sp_WEB_TopPlanAbnormalityPlant_GetChart", model);


        public async Task<IEnumerable<TopPlanAbnormalityCourseDto>> GetDataCourseAsync(TopPlanAbnormalityCourseParamDto model)
            => await dbExecutor.QueryListAsync<TopPlanAbnormalityCourseDto>("sp_WEB_TopPlanAbnormalityCourse_GetData", model);

        public async Task<IEnumerable<TopPlanAbnormalityCourseDto>> GetChartByCourse(TopPlanAbnormalityCourseParamDto model)
            => await dbExecutor.QueryListAsync<TopPlanAbnormalityCourseDto>("sp_WEB_TopPlanAbnormalityCourse_GetChart", model);

        public async Task<IEnumerable<TopPlanAbnormalityInspectionDto>> GetDataInspectionAsync(TopPlanAbnormalityInspectionParamDto model)
            => await dbExecutor.QueryListAsync<TopPlanAbnormalityInspectionDto>("sp_WEB_TopPlanAbnormalityInspection_GetData", model);
    }
}

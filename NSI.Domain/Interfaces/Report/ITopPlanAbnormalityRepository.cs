using NSI.Domain.DTOs.Report;

namespace NSI.Domain.Interfaces.Report
{
    public interface ITopPlanAbnormalityRepository
    {
        Task<IEnumerable<TopPlanAbnormalityPlantDto>> GetDataPlantAsync(TopPlanAbnormalityPlantParamDto model);
        Task<IEnumerable<TopPlanAbnormalityPlantDto>> GetChartByPlant(TopPlanAbnormalityPlantParamDto model);
        Task<IEnumerable<TopPlanAbnormalityCourseDto>> GetDataCourseAsync(TopPlanAbnormalityCourseParamDto model);
        Task<IEnumerable<TopPlanAbnormalityCourseDto>> GetChartByCourse(TopPlanAbnormalityCourseParamDto model);
        Task<IEnumerable<TopPlanAbnormalityInspectionDto>> GetDataInspectionAsync(TopPlanAbnormalityInspectionParamDto model);
    }
}
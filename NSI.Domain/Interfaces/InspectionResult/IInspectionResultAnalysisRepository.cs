using NSI.Domain.DTOs.Inspection;
using NSI.Domain.DTOs.InspectionResult;

namespace NSI.Domain.Interfaces.InspectionResult
{
    public interface IInspectionResultAnalysisRepository
    {
        Task<List<UserPlantDto>> GetUserPlant(string UserID);

        Task<List<InspectionResultAnalysisDto>> GetData(InspectionResultParamDto model);
        
        Task<List<InspectionResultAnalysisChartDto>> GetChart(InspectionResultParamDto model);

        Task<List<InspectionGetUnitDto>> GetUnitName(InspectionResultParamDto model);

    }
}

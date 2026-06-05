using NSI.Domain.DTOs.Inspection;
using NSI.Domain.DTOs.InspectionResult;

namespace NSI.Domain.Interfaces.InspectionResult
{
    public interface IInspectionResultSummaryByGroupRepository
    {

        Task<List<InspectionResultSummaryByGroupDto>> GetData(InspectionResultByGroupParamDto model);


    }
}

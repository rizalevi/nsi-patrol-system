using NSI.Domain.DTOs.Inspection;
using NSI.Domain.DTOs.Security;

namespace NSI.Domain.Interfaces.Inspection
{
    public interface IInspectionDataDDLRepository
    {
        Task<List<InspectionDataDDLDto>> GetClusterDDL();
        Task<List<InspectionDataDDLDto>> GetPlantDDL(string clusterCode);
        Task<List<InspectionDataDDLDto>> GetCourseDDL(string plantCode);

        Task<List<InspectionDataDDLDto>> GetGroupDDL(string courseCode,string plantCode);
        Task<List<InspectionDataDDLDto>> GetInspectionPointDDL(string courseCode,string groupCode);
        Task<List<InspectionDataDDLDto>> GetFrequencyDDL();
        Task<List<InspectionDataDDLDto>> GetInspectionPointForChartDDL(string courseCode);
		Task<List<InspectionDataUnitDDLDto>> GetUnitDDL();
        Task<List<InspectionDataUnitOptionDDLDto>> GetUnitOptionDDL(string unitCode);

    }
}

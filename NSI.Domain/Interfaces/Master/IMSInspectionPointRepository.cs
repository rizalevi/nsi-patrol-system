using NSI.Domain.DTOs.Master;
using NSI.Domain.Models.Master;

namespace NSI.Domain.Interfaces.Master
{
    public interface IMSInspectionPointRepository
    {
        //Task<List<MSInspectionPointDto>> GetClusters();
        //Task<List<MSInspectionPointDto>> GetPlants(string clusterCode); 
        //Task<List<MSInspectionPointDto>> GetCourses(string plantCode);
        //Task<List<MSInspectionPointDto>> GetGroups(string courseCode);

        Task<List<MSInspectionPointDto>> GetList(MSInpectionPointParam model);
        Task<List<MSInspectionPointDDLDto>> GetCharts();
        Task<List<MSInspectionPointUnitDDLDto>> GetUnits();
        Task<List<MSInspectionPointMethodDDLDto>> GetMethode(string unitCode);

        ///Task<List<MSInspectionPointDto>> GetAll(string clusterCode);
        Task<MSInspectionPointDto> GetData(MSInpectionPointDetailParam model);
        //Task<MSInspectionPointDto> CheckInspectionCode(string courseCode, string groupCode, string inspectionCode);
        //Task<MSInspectionPointDto> GenerateInspectionCode(string courseCode, string groupCode);
      
       
        Task Create(MSInspectionPoint model, string userId);
        Task Update(MSInspectionPoint payload, string userId);
        Task Remove(string courseCode, string groupCode, string inspectionCode, string userId,string ipAddress); 
    }
}

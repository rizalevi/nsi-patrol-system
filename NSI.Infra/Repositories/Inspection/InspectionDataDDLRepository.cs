using Dapper;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.DTOs.Security;
using NSI.Domain.Interfaces;
using NSI.Domain.Interfaces.Inspection;
using NSI.Domain.Interfaces.Security;
using NSI.Domain.Models.Security ;
using NSI.Infra.Datas;
using NSI.Infra.Helpers;

namespace NSI.Infra.Repositories.Inspection
{
    public class InspectionDataDDLRepository : IInspectionDataDDLRepository
    {
        private readonly DbExecutor _dbExecutor;
       
        public InspectionDataDDLRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<List<InspectionDataDDLDto>> GetClusterDDL()
        {
            string sp = "sp_WEB_InspectionData_Combo";
            var result = await _dbExecutor.QueryListAsync<InspectionDataDDLDto>(sp, new {Type="0"});
            return result.ToList();
        }

        public async Task<List<InspectionDataDDLDto>> GetPlantDDL(string clusterCode)
        {
            string sp = "sp_WEB_InspectionData_Combo";
            var result = await _dbExecutor.QueryListAsync<InspectionDataDDLDto>(sp, new { Type = "1", Param1 = clusterCode });
            return result.ToList();
        }


        public async Task<List<InspectionDataDDLDto>> GetCourseDDL(string plantCode)
        {
            string sp = "sp_WEB_InspectionData_Combo";
            var result = await _dbExecutor.QueryListAsync<InspectionDataDDLDto>(sp, new { Type = "2" , Param1 = plantCode});
            return result.ToList();

        }

        public async Task<List<InspectionDataDDLDto>> GetGroupDDL(string courseCode, string plantCode)
        {
            string sp = "sp_WEB_InspectionData_Combo";
            var result = await _dbExecutor.QueryListAsync<InspectionDataDDLDto>(sp, new { Type = "3",Param1 = courseCode,  Param2= plantCode });
            return result.ToList();
        }

        public async Task<List<InspectionDataDDLDto>> GetInspectionPointDDL(string courseCode, string groupCode)
        {
            string sp = "sp_WEB_InspectionData_Combo";
            var result = await _dbExecutor.QueryListAsync<InspectionDataDDLDto>(sp, new { Type = "4", Param1 = courseCode, Param2 = groupCode });
            return result.ToList();
        }

        public async Task<List<InspectionDataDDLDto>> GetFrequencyDDL()
        {
            string sp = "sp_WEB_InspectionData_Combo";
            var result = await _dbExecutor.QueryListAsync<InspectionDataDDLDto>(sp, new { Type = "5" });
            return result.ToList();
        }

        public async Task<List<InspectionDataDDLDto>> GetInspectionPointForChartDDL(string courseCode)
        {
            string sp = "sp_WEB_InspectionData_Combo";
            var result = await _dbExecutor.QueryListAsync<InspectionDataDDLDto>(sp, new { Type = "6", Param1 = courseCode });
            return result.ToList();
        }

		public async Task<List<InspectionDataUnitDDLDto>> GetUnitDDL()
        {
            string sp = "sp_WEB_InspectionData_Combo";
            var result = await _dbExecutor.QueryListAsync<InspectionDataUnitDDLDto>(sp, new { Type = "7" });
            return result.ToList();
        }

        public async Task<List<InspectionDataUnitOptionDDLDto>> GetUnitOptionDDL(string unitCode)
        {
            string sp = "sp_WEB_InspectionData_Combo";
            var result = await _dbExecutor.QueryListAsync<InspectionDataUnitOptionDDLDto>(sp, new { Type = "8", Param1 = unitCode });
            return result.ToList();
        }

    }
}

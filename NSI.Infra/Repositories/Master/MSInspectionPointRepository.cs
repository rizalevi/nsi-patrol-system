using NSI.Domain.DTOs.Master;
using NSI.Domain.Interfaces;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Master;
using NSI.Infra.Datas;

namespace NSI.Infra.Repositories.Master
{
    public class MSInspectionPointRepository : IMSInspectionPointRepository
    {
        private readonly DbExecutor _dbExecutor;

        public MSInspectionPointRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<List<MSInspectionPointDto>> GetList(MSInpectionPointParam model)
        {
            string sp = "SP_Web_MSInspectionPoint_GetList";
            return (await _dbExecutor.QueryListAsync<MSInspectionPointDto>(sp,model)).ToList();
        }

        #region Combo Fill
        public async Task<List<MSInspectionPointDDLDto>> GetCharts()
        {
            string sp = "sp_Web_MSInspectionPoint_Combo";
            return (await _dbExecutor.QueryListAsync<MSInspectionPointDDLDto>(sp, new { Type = 1 })).ToList();
        }

        public async Task<List<MSInspectionPointUnitDDLDto>> GetUnits()
        {
            string sp = "sp_Web_MSInspectionPoint_Combo";
            return (await _dbExecutor.QueryListAsync<MSInspectionPointUnitDDLDto>(sp, new { Type = 2 })).ToList();
        }
        //public async Task<List<MSInspectionPointDto>> GetClusters()
        //{
        //    string sp = "sp_Web_MSInspectionPoint_Combo";
        //    return (await _dbExecutor.QueryListAsync<MSInspectionPointDto>(sp, new { Type = 1 })).ToList();
        //}

        //public async Task<List<MSInspectionPointDto>> GetPlants(string clusterCode)
        //{
        //    string sp = "sp_Web_MSInspectionPoint_Combo";
        //    return (await _dbExecutor.QueryListAsync<MSInspectionPointDto>(sp, new { Type = 2, ClusterCode = clusterCode })).ToList();
        //}

        //public async Task<List<MSInspectionPointDto>> GetCourses(string plantCode)
        //{
        //    string sp = "sp_Web_MSInspectionPoint_Combo";
        //    return (await _dbExecutor.QueryListAsync<MSInspectionPointDto>(sp, new { Type = 5, PlantCode = plantCode })).ToList();
        //}

        //public async Task<List<MSInspectionPointDto>> GetGroups(string courseCode)
        //{
        //    string sp = "sp_Web_MSInspectionPoint_Combo";
        //    return (await _dbExecutor.QueryListAsync<MSInspectionPointDto>(sp, new { Type = 6, CourseCode = courseCode })).ToList();
        //}



        #endregion

        //public async Task<List<MSInspectionPointDto>> GetAll(string clusterCode)
        //{
        //    string sp = "sp_Web_MSInspectionPoint_Get_InspectionPoint";
        //    return (await _dbExecutor.QueryListAsync<MSInspectionPointDto>(sp, new { ClusterCode = clusterCode })).ToList();
        //}

        public async Task<MSInspectionPointDto> GetData(MSInpectionPointDetailParam model)
        {
            string sp = "SP_Web_MSInspectionPoint_GetData";
            return await _dbExecutor.QueryFirstOrDefaultAsync<MSInspectionPointDto>(sp, model);
        }

        //public async Task<MSInspectionPointDto> CheckInspectionCode(MSInpectionPointParam model)
        //{
        //    string sp = "SP_Web_MSInspectionPoint_CheckInspectionCode";
        //    return await _dbExecutor.QueryFirstOrDefaultAsync<MSInspectionPointDto>(sp, model);
        //}

        //public async Task<MSInspectionPointDto> GenerateInspectionCode(MSInpectionPointParam model)
        //{
        //    string sp = "SP_Web_MSInspectionPoint_AutoGenerateInspectionCode";
        //    return await _dbExecutor.QueryFirstOrDefaultAsync<MSInspectionPointDto>(sp, model);
        //}

        public async Task<List<MSInspectionPointMethodDDLDto>> GetMethode(string unitCode)
        {
            string sp = "sp_Web_MSInspectionPoint_GetMethode";
            return (await _dbExecutor.QueryListAsync<MSInspectionPointMethodDDLDto>(sp, new { UnitCode = unitCode })).ToList();
        }

        public async Task Create(MSInspectionPoint model, string userId)
        {
            string sql = @"SP_Web_MSInspectionPoint_Insert";
            await _dbExecutor.ExecuteAsync(sql, new
            {
                CourseCode = model.CourseCode,
                GroupCode = model.GroupCode,
                InspectionCode = model.InspectionCode,
                InspectionPoint = model.InspectionPoint,
                UnitCode = model.UnitCode,  
                Minimum = model.Minimum, 
                Maximum = model.Maximum, 
                Chart = model.Chart, 
                Option = model.Option,
                Status = model.Status,
                UserID = userId
            });
        }

        public async Task Update(MSInspectionPoint model, string userId)
        {
            string sql = @"SP_Web_MSInspectionPoint_Update";
            await _dbExecutor.ExecuteAsync(sql, new
            {
                CourseCode = model.CourseCode,
                GroupCode = model.GroupCode,
                InspectionCode = model.InspectionCode,
                InspectionPoint = model.InspectionPoint,
                UnitCode = model.UnitCode,
                Minimum = model.Minimum,
                Maximum = model.Maximum,
                Chart = model.Chart,
                Option = model.Option,
                Status = model.Status,
                UserID = userId
            });
        }

        public async Task Remove(string courseCode, string groupCode, string inspectionCode, string userID,string ipAddress)
        {
            string sql = "sp_Web_MSInspectionPoint_Delete";
            await _dbExecutor.ExecuteAsync(sql, new { CourseCode = courseCode, GroupCode = groupCode, InspectionCode = inspectionCode, UserID = userID , IPAddress = ipAddress });
        }

    }
}

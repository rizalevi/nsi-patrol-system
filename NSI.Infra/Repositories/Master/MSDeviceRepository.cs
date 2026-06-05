using Dapper;
using NSI.Domain.DTOs.Master;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Master;
using NSI.Infra.Datas;
using NSI.Infra.Helpers;
using System.Data;

namespace NSI.Infra.Repositories.Master
{
    public class MSDeviceRepository : IMSDeviceRepository
    {
        private readonly DbExecutor _dbExecutor;

        public MSDeviceRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }


        #region MS device
        public async Task<List<MSDeviceDto>> GetList()
        {
            string sp = "sp_Web_MSDevice_GetMaster";
            return (await _dbExecutor.QueryListAsync<MSDeviceDto>(sp)).ToList();
        }

        public async Task<MSDeviceDto> GetData(string Code)
        {
            string sp = "sp_Web_MSDevice_GetDeviceByCode";
            return await _dbExecutor.QueryFirstOrDefaultAsync<MSDeviceDto>(sp, new { DeviceCode = Code });
        }

        public async Task Create(MSDevice model, string userId)
        {
            string sql = @"sp_Web_MSDevice_InsertMaster";
            await _dbExecutor.ExecuteAsync(sql, new
            {
                DeviceCode = model.DeviceCode,
                DeviceName = model.DeviceName,
                PlantCode = model.PlantCode,
                SerialNo = model.SerialNo,
                UserID = userId
            });
        }

        public async Task Update(MSDevice model, string userId)
        {
            string sql = @"sp_Web_MSDevice_UpdateMaster";
            await _dbExecutor.ExecuteAsync(sql, new
            {
                DeviceCode = model.DeviceCode,
                DeviceName = model.DeviceName,
                PlantCode = model.PlantCode,
                SerialNo = model.SerialNo,
                UserID = userId
            });
        }

        public async Task Remove(string Code, string userID, string ipAddress)
        {
            string sql = "sp_Web_MSDevice_DeleteMaster";
            await _dbExecutor.ExecuteAsync(sql, new { DeviceCode = Code, UserID = userID, IPAddress = ipAddress });
        }

        public async Task<List<MSPlantDto>> GetPlants()
        {
            string sp = "sp_Web_MSDevice_Combo";
            return (await _dbExecutor.QueryListAsync<MSPlantDto>(sp, new { Type = 1 })).ToList();
        }
        #endregion

        #region MS device detail
        public async Task<List<MSDeviceDetailDto>> GetListDetail(string Code, string plantCode)
        {
            string sp = "sp_Web_MSDevice_GetDetail";
            return (await _dbExecutor.QueryListAsync<MSDeviceDetailDto>(sp, new { DeviceCode = Code, PlantCode = plantCode})).ToList();
        } 

        //public async Task SubmitDetail(MSDeviceDetail model, string userId)
        //{
        //    string sql = @"sp_Web_MSDevice_InsertDetail";
        //    await _dbExecutor.ExecuteAsync(sql, new
        //    {
        //        DeviceCode = model.DeviceCode,
        //        PlantCode = model.PlantCode,
        //        CourseCode = model.CourseCode ,
        //        UserID=userId
        //    });
        //}

        public async Task SubmitBatch(MSDeviceBatch model, string userId)
        {
            var param = new DynamicParameters();
            // 🔥 TVP
            param.Add("@ListDetailBatch",
            DataTableHelper.SubmitDeviceBatch(model?.SubmitBatch).AsTableValuedParameter("dbo.DeviceDetailBatch"));
            param.Add("@UserID", userId);

            await _dbExecutor.ExecuteAsync(
                "sp_Web_MSDevice_SubmitBatch",
                param,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 120
            );
        }

        //public async Task RemoveHTDetail(string hTCode, string plantCode, string courseCode, string userID, string ipAddress)
        //{
        //    string sql = "sp_Web_MSHT_HTDetail_Delete";
        //    await _dbExecutor.ExecuteAsync(sql, new { HTCode = hTCode, PlantCode = plantCode, CourseCode = courseCode, UserID = userID });
        //}

        //public async Task<List<MSHTDetailDto>> GetPlantsHTDetail()
        //{
        //    string sp = "sp_Web_MSHT_Combo";
        //    return (await _dbExecutor.QueryListAsync<MSHTDetailDto>(sp, new { Type = 1 })).ToList();
        //}

        public async Task<List<MSDeviceDDLDto>> GetDeviceMaster()
        {
            string sp = "sp_Web_MSDevice_Combo";
            return (await _dbExecutor.QueryListAsync<MSDeviceDDLDto>(sp, new { Type = 2 })).ToList();
        }

        #endregion
    }
}

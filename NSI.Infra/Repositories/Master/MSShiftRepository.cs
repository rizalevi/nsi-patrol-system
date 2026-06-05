using NSI.Domain.DTOs.Master;
using NSI.Domain.Interfaces.Master;
using NSI.Infra.Datas;

namespace NSI.Infra.Repositories.Master
{
    public class MSShiftRepository : IMSShiftRepository
    {
        private readonly DbExecutor _dbExecutor;

        public MSShiftRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<List<MSShiftDto>> GetList()
        {
            string sp = "sp_Web_MSShift_GetShift";
            return (await _dbExecutor.QueryListAsync<MSShiftDto>(sp)).ToList();
        }
          
        public async Task<MSShiftDto> GetData(string shiftCls)
        {
            string sp = "sp_Web_MSShift_GetShiftDetail";
            return await _dbExecutor.QueryFirstOrDefaultAsync<MSShiftDto>(sp, new { Shift_Cls = shiftCls});
        } 

        public async Task Create(MSShiftDto model, string userId)
        {
            string sql = @"sp_Web_MSShift_InsertShift";
            await _dbExecutor.ExecuteAsync(sql, new
            { 
                Shift_Cls = model.Shift_Cls,
                Inspection = model.Inspection_Time,
                RegisterUser = userId, 
                Last_User = userId
            });
        }

        public async Task Update(MSShiftDto model, string userId)
        {
            string sql = @"[sp_Web_MSShift_UpdateShift]";
            await _dbExecutor.ExecuteAsync(sql, new
            { 
                Shift_Cls = model.Shift_Cls,
                Inspection = model.Inspection_Time,
                Last_User = userId
            });
        }

        public async Task Remove(string shiftCls, string userID)
        {
            string sql = "[sp_Web_MSShift_DeleteShift]";
            await _dbExecutor.ExecuteAsync(sql, new { Shift_Cls = shiftCls, UserID = userID });
        }

    }
}

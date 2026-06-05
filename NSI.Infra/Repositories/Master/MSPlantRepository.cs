using NSI.Domain.DTOs.Master;
using NSI.Domain.Interfaces;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Master;
using NSI.Infra.Datas;

namespace NSI.Infra.Repositories.Master
{
    public class MSPlantRepository : IMSPlantRepository
    {
        private readonly DbExecutor _dbExecutor;
       
        public MSPlantRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }


        public async Task<List<MSPlantDto>> GetAll(string clusterCode)
        {
            string sp = "sp_Web_MSPlant_List";
            return (await _dbExecutor.QueryListAsync<MSPlantDto>(sp,new {ClusterCode = clusterCode})).ToList();
        }

        public async Task<MSPlant> GetData(string clusterCode, string plantCode)
        {
            string sp = "sp_Web_MSPlant_GetData";
            return await _dbExecutor.QueryFirstOrDefaultAsync<MSPlant>(sp, new { ClusterCode = clusterCode,  PlantCode = plantCode });
        }

        public async Task Create(MSPlant model, string userId)
        {
            // address.AddressCode = await _dbExecutor.QuerySingleOrDefaultAsync<string>("sp_Wms_Address_GenerateCode", new { address.WarehouseCode, address.AreaCode });
            string sql = @"sp_Web_MSPlant_Insert";
            await _dbExecutor.ExecuteAsync(sql, new
            {
                model.ClusterCode,
                model.PlantCode,
                model.Description,
                UserID = userId
            });
        }

        public async Task Update(MSPlant model, string userId)
        {
            string sql = @"sp_Web_MSPlant_Update";
            await _dbExecutor.ExecuteAsync(sql, new
            {
                model.ClusterCode,
                model.PlantCode,
                model.Description,
                UserID = userId
            });
        }

        public async Task Remove(string clusterCode,string plantCode, string userId, string ipAddress)
        {
            string sql = "sp_Web_MSPlant_Delete";
            await _dbExecutor.ExecuteAsync(sql, new { ClusterCode = clusterCode , PlantCode = plantCode, UserID = userId , IPAddress = ipAddress });
        }

    }
}

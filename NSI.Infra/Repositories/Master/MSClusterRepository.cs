using NSI.Domain.DTOs.Master;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Master;
using NSI.Infra.Datas;

namespace NSI.Infra.Repositories.Master
{
    public class MSClusterRepository : IMSClusterRepository
    {
        private readonly DbExecutor _dbExecutor;
       
        public MSClusterRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }


        public async Task<List<MSClusterDto>> GetAll()
        {
            string sp = "sp_Web_MSCluster_List";
            return (await _dbExecutor.QueryListAsync<MSClusterDto>(sp)).ToList();
        }

        public async Task<MSCluster> GetData(string clusterCode)
        {
            string sp = "sp_Web_MSCluster_GetData";
            return await _dbExecutor.QueryFirstOrDefaultAsync<MSCluster>(sp, new { ClusterCode = clusterCode });
        }

        public async Task Create(MSCluster model, string userId)
        {
            // address.AddressCode = await _dbExecutor.QuerySingleOrDefaultAsync<string>("sp_Wms_Address_GenerateCode", new { address.WarehouseCode, address.AreaCode });
            string sql = @"sp_Web_MSCluster_Insert";
            await _dbExecutor.ExecuteAsync(sql, new
            {
                model.ClusterCode,
                model.Description,
                UserID = userId
            });
        }

        public async Task Update(MSCluster model, string userId)
        {
            string sql = @"sp_Web_MSCluster_Update";
            await _dbExecutor.ExecuteAsync(sql, new
            {
                model.ClusterCode,
                model.Description,
                UserID = userId
            });
        }

        public async Task Remove(string clusterCode, string userId, string ipAddress)
        {
            string sql = "sp_Web_MSCluster_Delete";
            await _dbExecutor.ExecuteAsync(sql, new { ClusterCode = clusterCode , UserID = userId , IPAddress = ipAddress });
        }

    }
}

using NSI.Domain.DTOs.Master;
using NSI.Domain.Interfaces;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Master;

namespace NSI.Api.Services.Master
{
    public class MSClusterRepositories 
    {
        private readonly IMSClusterRepository _repo;

        public MSClusterRepositories(IMSClusterRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<MSClusterDto>> GetAll()
        {
            var data = await _repo.GetAll();
            return data.Select(x => new MSClusterDto
            {
                ClusterCode = x.ClusterCode,
                Description = x.Description,
                RegisterDate = x.RegisterDate,
                RegisterUser = x.RegisterUser,
                LastUpdate = x.LastUpdate,
                LastUser = x.LastUser
            });
        }

        public async Task<MSCluster> GetData(string code)
        {
            var data = await _repo.GetData(code);
            if (data == null) return null;

            return new MSCluster
            {
                ClusterCode = data.ClusterCode,
                Description = data.Description
            };
        }

        public async Task Create(MSCluster model, string userId)
        {
            var entity = new MSCluster
            {
                ClusterCode = model.ClusterCode,
                Description = model.Description
            };

            await _repo.Create(entity, userId);
        }

        public async Task Update(MSCluster model, string userId)
        {
            var entity = new MSCluster
            {
                ClusterCode = model.ClusterCode,
                Description = model.Description
            };

            await _repo.Update(entity, userId);
        }

        public async Task Remove(string code, string userId, string ipAddress)
        {
            await _repo.Remove(code, userId, ipAddress);
        }



    }
}

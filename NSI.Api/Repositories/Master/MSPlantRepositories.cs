using NSI.Domain.DTOs.Master;
using NSI.Domain.Interfaces;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Master;

namespace NSI.Api.Services.Master
{
    public class MSPlantRepositories
    {
        private readonly IMSPlantRepository _repo;

        public MSPlantRepositories(IMSPlantRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<MSPlantDto>> GetAll(string clusterCode)
        {
            var data = await _repo.GetAll( clusterCode);
            return data.Select(x => new MSPlantDto
            {
                PlantCode = x.PlantCode,
                Description = x.Description,
                RegisterDate = x.RegisterDate,
                RegisterUser = x.RegisterUser,
                LastUpdate = x.LastUpdate,
                LastUser = x.LastUser
            });
        }

        public async Task<MSPlant> GetData(string clusterCode,string code)
        {
            var data = await _repo.GetData(clusterCode,code);
            if (data == null) return null;

            return new MSPlant
            {
                PlantCode= data.PlantCode,
                Description = data.Description,
                ClusterCode= data.ClusterCode,
                ClusterName= data.ClusterName
            };
        }

        public async Task Create(MSPlant model, string userId)
        {
            var entity = new MSPlant
            {
                PlantCode = model.PlantCode,
                Description = model.Description,
                ClusterCode = model.ClusterCode
            };

            await _repo.Create(entity, userId);
        }

        public async Task Update(MSPlant model, string userId)
        {
            var entity = new MSPlant
            {
                PlantCode = model.PlantCode,
                Description = model.Description,
                ClusterCode = model.ClusterCode
            };

            await _repo.Update(entity, userId);
        }

        public async Task Remove(string clusterCode,string code, string userId, string ipAddress)
        {
            await _repo.Remove(clusterCode,code, userId, ipAddress);
        }



    }
}

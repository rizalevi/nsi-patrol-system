using NSI.Domain.DTOs.Master;
using NSI.Domain.Models.Master;

namespace NSI.Domain.Interfaces.Master
{
    public interface IMSPlantRepository
    {
        Task<List<MSPlantDto>> GetAll(string clusterCode);
        Task<MSPlant?> GetData(string clusterCode,string code);
        Task Create(MSPlant model, string userId);
        Task Update(MSPlant payload , string userId);
        Task Remove(string clusterCode,string code, string userId, string ipAddress);
        //Task<Dictionary<string, object>> Capture(string addressCode);

    }
}

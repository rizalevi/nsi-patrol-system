using NSI.Domain.DTOs.Master;
using NSI.Domain.Models.Master;

namespace NSI.Domain.Interfaces.Master
{
    public interface IMSClusterRepository
    {
        Task<List<MSClusterDto>> GetAll();
        Task<MSCluster?> GetData(string code);
        Task Create(MSCluster model, string userId);
        Task Update(MSCluster payload , string userId);
        Task Remove(string code, string userId, string ipAddress);
        //Task<Dictionary<string, object>> Capture(string addressCode);

    }
}

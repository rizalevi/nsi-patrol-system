using NSI.Domain.DTOs.Master;
using NSI.Domain.Models.Master;

namespace NSI.Domain.Interfaces.Master
{
    public interface IMSUnitClassificationRepository
    {
        Task<List<MSUnitClassificationDto>> GetAll();
        Task<MSUnitClassification?> GetData(string unitCode);
        Task Create(MSUnitClassification model, string userId);
        Task Update(MSUnitClassification payload, string userId);
        Task Remove(string unitCode, string userId, string ipAddress);
    }
}
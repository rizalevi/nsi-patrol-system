using NSI.Domain.DTOs.Master;
using NSI.Domain.Models.Master;

namespace NSI.Domain.Interfaces.Master
{
    public interface IMSShiftRepository
    {
        Task<List<MSShiftDto>> GetList();  
        Task<MSShiftDto> GetData(string shiftCls); 
        Task Create(MSShiftDto model, string userId);
        Task Update(MSShiftDto payload, string userId);
        Task Remove(string shiftCls, string userId);
    }
}

using NSI.Domain.DTOs.Master;
using NSI.Domain.Models.Inspection;
using NSI.Domain.Models.Master;

namespace NSI.Domain.Interfaces.Master
{
    public interface IMSDeviceRepository
    {
        #region MSHT
        Task<List<MSDeviceDto>> GetList();
        Task<MSDeviceDto> GetData(string Code);
        Task Create(MSDevice model, string userId);
        Task Update(MSDevice payload, string userId);
        Task Remove(string Code, string userId,string ipAddress); 
        Task<List<MSPlantDto>> GetPlants();
        #endregion

        #region MS device detail
        Task<List<MSDeviceDetailDto>> GetListDetail(string Code, string plantCode); 
        Task SubmitBatch(MSDeviceBatch model, string userId); 
        //Task RemoveHTDetail(string Code, string plantCode, string courseCode, string userId);
        Task<List<MSDeviceDDLDto>> GetDeviceMaster();
                
        #endregion
    }
}

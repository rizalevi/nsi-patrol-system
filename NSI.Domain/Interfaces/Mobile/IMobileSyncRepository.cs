using NSI.Domain.DTOs.Mobile; 

namespace NSI.Domain.Interfaces.Mobile
{
    public interface IMobileSyncRepository
    {
        Task<MobileSyncDto?> GetSync(string htCode, string userLogin, DateTime? lastSync);
    }
}

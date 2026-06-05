using NSI.Domain.DTOs.Mobile; 

namespace NSI.Domain.Interfaces.Mobile
{
    public interface IMobileCheckConnectionRepository
    {
        Task<MobileCheckConnectionDto?> GetData();
    }
}



using NSI.Domain.DTOs.Mobile;

namespace NSI.Domain.Interfaces.Mobile
{
    public interface IMobileLastVersionRepository
    {
        Task<MobileLastVersionDto?> GetVersion();
    }
}

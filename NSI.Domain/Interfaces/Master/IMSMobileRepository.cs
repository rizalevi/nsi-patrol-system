

using NSI.Domain.DTOs.Master;
using NSI.Domain.Models.Master;

namespace NSI.Domain.Interfaces.Master
{
    public interface IMSMobileRepository
    {
        Task<List<MSMobileDto>> GetAll(DateTime? lastSync);
        Task Upset(MSMobile payload);
    }
}

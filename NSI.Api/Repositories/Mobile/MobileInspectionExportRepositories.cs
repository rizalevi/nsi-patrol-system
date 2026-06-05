using NSI.Domain.DTOs.Mobile;
using NSI.Domain.Interfaces.Mobile;

namespace NSI.Api.Repositories.Mobile
{
    public class MobileInspectionExportRepositories
    {
        private readonly IMobileInspectionExportRepository _repo;
        public MobileInspectionExportRepositories(IMobileInspectionExportRepository repo)
        {
            _repo = repo;
        }
        public async Task<bool> SyncExport(MobileInspectionExportDto data)
        {
            return await _repo.SyncExport(data);
        }
    }
}

using NSI.Domain.DTOs.Mobile; 

namespace NSI.Domain.Interfaces.Mobile
{
    public interface IMobileInspectionExportRepository
    {
        Task<bool> SyncExport(MobileInspectionExportDto data);
    }
} 
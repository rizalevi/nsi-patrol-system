using NSI.Domain.DTOs.Master;
using NSI.Domain.Models.Master;

namespace NSI.Domain.Interfaces.Master
{
    public interface IMSEquipmentGroupRepository
    {
        // Menggunakan IEnumerable dan akhiran Async
        Task<IEnumerable<MSEquipmentGroupDto>> GetAllAsync(string courseCode = "", string plantCode = "");

        // Mengambil data tunggal berdasarkan primary key / unique identifier
        Task<MSEquipmentGroup?> GetDataAsync(string groupCode);

        Task CreateAsync(MSEquipmentGroup model, string userId);

        Task UpdateAsync(MSEquipmentGroup payload, string userId);

        // Menyesuaikan parameter untuk proses delete sesuai kebutuhan log/history
        Task RemoveAsync(string groupCode, string courseCode, string userId, string ipAddress);

        // Method untuk combo box filter (berdasarkan alur dari kode VB: Cluster -> Plant -> Course)
        Task<IEnumerable<MSEquipmentGroupFilterClusterDto>> GetFilterClusterAsync();

        Task<IEnumerable<MSEquipmentGroupFilterPlantDto>> GetFilterPlantAsync(string clusterCode);

        Task<IEnumerable<MSEquipmentGroupFilterCourseDto>> GetFilterCourseAsync(string plantCode);

        // Method khusus dari VB untuk Auto Generate kode Group
        Task<string> GenerateGroupCodeAsync(string courseCode);
    }
}
using NSI.Domain.DTOs.Master;
using NSI.Domain.Models.Master;

namespace NSI.Domain.Interfaces.Master
{
    public interface IMSCourseRepository
    {
        // Menggunakan IEnumerable dan akhiran Async
        Task<IEnumerable<MSCourseDto>> GetAllAsync(string ClusterCode, string PlantCode);

        // Menyesuaikan parameter 'code' menjadi 'plantCode' agar jelas
        Task<MSCourse?> GetDataAsync(string ClusterCode, string PlantCode, string CourseCode);

        Task CreateAsync(MSCourse model, string userId);

        Task UpdateAsync(MSCourse payload, string userId);

        Task RemoveAsync(string clusterCode, string plantCode, string courseCode, string userId, string ipAddress);

        Task<IEnumerable<MSCourseFilterClusterDto>> GetFilterClusterAsync();

        Task<IEnumerable<MSCourseFilterPlantDto>> GetFilterPlantAsync(string clusterCode);

        Task<IEnumerable<MSCourseFilterFrequencyDto>> GetFilterFrequencyAsync();
    }
}
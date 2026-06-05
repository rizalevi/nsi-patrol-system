using NSI.Domain.DTOs.Master;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Master;
using NSI.Infra.Datas;

namespace NSI.Infra.Repositories.Master
{
    public class MSCourseRepository : IMSCourseRepository
    {
        private readonly DbExecutor _dbExecutor;

        public MSCourseRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        // Tambahkan Async, dan ubah List menjadi IEnumerable (hilangkan .ToList())
        public async Task<IEnumerable<MSCourseDto>> GetAllAsync(string clusterCode, string plantCode)
        {
            string sp = "sp_Web_MSCourse_List";
            return await _dbExecutor.QueryListAsync<MSCourseDto>(sp, new { ClusterCode = clusterCode, PlantCode = plantCode });
        }

        // Tambahkan Async
        public async Task<MSCourse?> GetDataAsync(string ClusterCode, string PlantCode, string CourseCode)
        {
            string sp = "sp_Web_MSCourse_GetData";
            return await _dbExecutor.QueryFirstOrDefaultAsync<MSCourse>(sp, new { ClusterCode, PlantCode, CourseCode});
        }

        public async Task CreateAsync(MSCourse model, string userId)
        {
            await _dbExecutor.ExecuteAsync("sp_Web_MSCourse_Insert", new
            {
                CourseCode = model.CourseCode,
                Title = model.Title,
                DocNo = model.DocNo,
                PlantCode = model.PlantCode,
                CheckType = model.CheckType,
                //Status_Shift = model.StatusShift,
                ShiftCls = model.ShiftCls ?? "",
                UserID = userId
            });
        }

        // Tambahkan Async
        public async Task UpdateAsync(MSCourse model, string userId)
        {
            string sql = @"sp_Web_MSCourse_Update";
            await _dbExecutor.ExecuteAsync(sql, new
            {
                //model.ClusterCode,
                model.PlantCode,
                model.CourseCode,
                model.Title,
                model.DocNo,
                model.CheckType,
                //model.StatusShift,
                model.ShiftCls,
                UserID = userId
            });
        }

        // Tambahkan Async
        public async Task RemoveAsync(string clusterCode, string plantCode, string courseCode, string userId, string ipAddress)
        {
            string sql = "sp_Web_MSCourse_Delete";
            await _dbExecutor.ExecuteAsync(sql, new { ClusterCode = clusterCode, PlantCode = plantCode, CourseCode = courseCode, UserID = userId, IPAddress = ipAddress });
        }

        // Tambahkan Async, ubah List menjadi IEnumerable
        public async Task<IEnumerable<MSCourseFilterClusterDto>> GetFilterClusterAsync()
        {
            string sp = "sp_Web_MSCourse_Combo";
            return await _dbExecutor.QueryListAsync<MSCourseFilterClusterDto>(sp, new { Type = "0" });
        }

        // Tambahkan Async, ubah List menjadi IEnumerable
        public async Task<IEnumerable<MSCourseFilterPlantDto>> GetFilterPlantAsync(string clusterCode)
        {
            string sp = "sp_Web_MSCourse_Combo";
            return await _dbExecutor.QueryListAsync<MSCourseFilterPlantDto>(sp, new { Type = "1", ClusterCode = clusterCode });
        }

        // Tambahkan Async, ubah List menjadi IEnumerable
        public async Task<IEnumerable<MSCourseFilterFrequencyDto>> GetFilterFrequencyAsync()
        {
            string sp = "sp_Web_MSCourse_Combo";
            return await _dbExecutor.QueryListAsync<MSCourseFilterFrequencyDto>(sp, new { Type = "2" });
        }
    }
}
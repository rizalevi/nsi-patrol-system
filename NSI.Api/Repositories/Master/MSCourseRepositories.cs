using NSI.Domain.DTOs.Master;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Master;

// Namespace disesuaikan dengan folder di gambarmu
namespace NSI.Api.Services.Master
{
    public class MSCourseRepositories(IMSCourseRepository repo)
    {

        // Menggunakan nama Async untuk konsistensi
        public async Task<IEnumerable<MSCourseDto>> GetAllAsync(string clusterCode, string plantCode) => await repo.GetAllAsync(clusterCode, plantCode);
        //{
        //    //var data = await _repo.GetAllAsync(clusterCode, plantCode);
        //    //return data.Select(x => new MSCourseDto
        //    //{
        //    //    PlantCode = x.PlantCode,
        //    //    CourseCode = x.CourseCode,
        //    //    Title = x.Title,
        //    //    DocNo = x.DocNo,
        //    //    Frequency = x.Frequency,
        //    //    ShiftCls = x.ShiftCls,
        //    //    RegisterDate = x.RegisterDate,
        //    //    RegisterUser = x.RegisterUser,
        //    //    LastUpdate = x.LastUpdate
        //    //});
        //    return await repo.GetAllAsync(clusterCode, plantCode);
        //}

        public async Task<MSCourse?> GetDataAsync(string clusterCode, string plantCode, string CourseCode) => (CourseCode == null) ? null : await repo.GetDataAsync(clusterCode, plantCode, CourseCode);
        //{
        //    //var data = await _repo.GetDataAsync(CourseCode);
        //    //if (data == null) return null;

        //    //return new MSCourse
        //    //{
        //    //    ClusterCode = data.ClusterCode,
        //    //    PlantCode = data.PlantCode,
        //    //    CourseCode = data.CourseCode,
        //    //    Title = data.Title,
        //    //    DocNo = data.DocNo,
        //    //    Frequency = data.Frequency,
        //    //    ShiftCls = data.ShiftCls,
        //    //};
        //    if (CourseCode == null) return null;
        //    return await repo.GetDataAsync(CourseCode);
        //}

        // Hapus async/await untuk menghemat memori (mengembalikan Task langsung)
        public Task CreateAsync(MSCourse model, string userId) => repo.CreateAsync(MapToEntity(model, userId), userId);
        //{
        //    var entity = MapToEntity(model, userId);
        //    return repo.CreateAsync(entity, userId);
        //}

        // Hapus async/await untuk menghemat memori
        public Task UpdateAsync(MSCourse model, string userId) => repo.UpdateAsync(MapToEntity(model, userId), userId);
        //{
        //    var entity = MapToEntity(model, userId);
        //    return repo.UpdateAsync(entity, userId);
        //}

        // Hapus async/await untuk menghemat memori
        public Task RemoveAsync(string code, string plantCode, string courseCode, string userId, string ipAddress) => repo.RemoveAsync(code, plantCode, courseCode, userId, ipAddress);
        //{
        //    return repo.RemoveAsync(code, plantCode, courseCode, userId, ipAddress);
        //}

        public async Task<IEnumerable<MSCourseFilterClusterDto>> GetFilterClusterAsync() => await repo.GetFilterClusterAsync();
        //{
        //    //var data = await _repo.GetFilterClusterAsync();
        //    //return data.Select(x => new MSCourseFilterClusterDto
        //    //{
        //    //    ClusterCode = x.ClusterCode,
        //    //    ClusterDesc = x.ClusterDesc,
        //    //});
        //    return await repo.GetFilterClusterAsync();
        //}

        public async Task<IEnumerable<MSCourseFilterPlantDto>> GetFilterPlantAsync(string clusterCode) => await repo.GetFilterPlantAsync(clusterCode);
        //{
        //    //var data = await _repo.GetFilterPlantAsync(clusterCode);
        //    //return data.Select(x => new MSCourseFilterPlantDto
        //    //{
        //    //    PlantCode = x.PlantCode,
        //    //    PlantDesc = x.PlantDesc,
        //    //});
        //    return await repo.GetFilterPlantAsync(clusterCode);
        //}

        public async Task<IEnumerable<MSCourseFilterFrequencyDto>> GetFilterFrequencyAsync() => await repo.GetFilterFrequencyAsync();
        //{
        //    //var data = await _repo.GetFilterFrequencyAsync();
        //    //return data.Select(x => new MSCourseFilterFrequencyDto
        //    //{
        //    //    FrequencyCode = x.FrequencyCode,
        //    //    FrequencyDesc = x.FrequencyDesc,
        //    //});
        //    return await repo.GetFilterFrequencyAsync();
        //}

        // Method pembantu (helper) agar kode pembuatan entity tidak berulang
        private static MSCourse MapToEntity(MSCourse model, string userId)
        {
            return new MSCourse
            {
                ClusterCode = model.ClusterCode,
                PlantCode = model.PlantCode,
                CourseCode = model.CourseCode,
                Title = model.Title,
                DocNo = model.DocNo,
                CheckType = model.CheckType,
                StatusShift = model.StatusShift,
                ShiftCls = model.ShiftCls,
                RegisterUser = userId,
            };
        }
    }
}
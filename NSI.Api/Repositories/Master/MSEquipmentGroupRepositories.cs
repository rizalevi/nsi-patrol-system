using NSI.Domain.DTOs.Master;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Master;
using System.Reflection.Metadata.Ecma335;

// Namespace disesuaikan dengan struktur Anda (sepertinya di layer Service/Business)
namespace NSI.Api.Services.Master
{
    public class MSEquipmentGroupRepositories(IMSEquipmentGroupRepository repo)
    {
        public async Task<IEnumerable<MSEquipmentGroupDto>> GetAllAsync(string CourseCode = "", string PlantCode = "") => await repo.GetAllAsync(CourseCode, PlantCode);
        //{
        //    //var data = await _repo.GetAllAsync(courseCode, plantCode);
        //    //return data.Select(x => new MSEquipmentGroupDto
        //    //{
        //    //    GroupCode = x.GroupCode,
        //    //    BarcodeNo = x.BarcodeNo,
        //    //    Description = x.Description,
        //    //    CourseCode = x.CourseCode,
        //    //    Title = x.Title,
        //    //    RegisterDate = x.RegisterDate,
        //    //    RegisterUser = x.RegisterUser,
        //    //    LastUpdate = x.LastUpdate,
        //    //    LastUser = x.LastUser
        //    //});
        //    //return await _repo.GetAllAsync(courseCode, plantCode);
        //}

        public async Task<MSEquipmentGroup?> GetDataAsync(string GroupCode) => await repo.GetDataAsync(GroupCode);
        //{
        //    var data = await _repo.GetDataAsync(groupCode);
        //    if (data == null) return null;

        //    return new MSEquipmentGroup
        //    {
        //        GroupCode = data.GroupCode,
        //        BarcodeNo = data.BarcodeNo,
        //        Description = data.Description,
        //        CourseCode = data.CourseCode,
        //        RegisterUser = data.RegisterUser,
        //        LastUser = data.LastUser
        //    };
        //}

        // Hapus async/await untuk menghemat memori (mengembalikan Task langsung)
        public Task CreateAsync(MSEquipmentGroup model, string userId) => repo.CreateAsync(MapToEntity(model, userId), userId);
        //{
        //    var entity = MapToEntity(model, userId);
        //    return _repo.CreateAsync(entity, userId);
        //}

        // Hapus async/await untuk menghemat memori
        public Task UpdateAsync(MSEquipmentGroup model, string userId) => repo.UpdateAsync(MapToEntity(model, userId), userId);
        //{
        //    var entity = MapToEntity(model, userId);
        //    return _repo.UpdateAsync(entity, userId);
        //}

        // Hapus async/await untuk menghemat memori
        public Task RemoveAsync(string groupCode, string courseCode, string userId, string ipAddress) => repo.RemoveAsync(groupCode, courseCode, userId, ipAddress);
        //{
        //    return _repo.RemoveAsync(groupCode, courseCode, userId, ipAddress);
        //}

        // --- Filter Combo Box ---

        public async Task<IEnumerable<MSEquipmentGroupFilterClusterDto>> GetFilterClusterAsync() => await repo.GetFilterClusterAsync();
        //{
        //    var data = await _repo.GetFilterClusterAsync();
        //    return data.Select(x => new MSEquipmentGroupFilterClusterDto
        //    {
        //        ClusterCode = x.ClusterCode,
        //        Description = x.Description
        //    });
        //}

        public async Task<IEnumerable<MSEquipmentGroupFilterPlantDto>> GetFilterPlantAsync(string ClusterCode) => await repo.GetFilterPlantAsync(ClusterCode);
        //{
        //    var data = await _repo.GetFilterPlantAsync(clusterCode);
        //    return data.Select(x => new MSEquipmentGroupFilterPlantDto
        //    {
        //        ClusterCode = x.ClusterCode,
        //        PlantCode = x.PlantCode,
        //        Description = x.Description
        //    });
        //}

        public async Task<IEnumerable<MSEquipmentGroupFilterCourseDto>> GetFilterCourseAsync(string PlantCode) => await repo.GetFilterCourseAsync(PlantCode);
        //{
        //    var data = await _repo.GetFilterCourseAsync(plantCode);
        //    return data.Select(x => new MSEquipmentGroupFilterCourseDto
        //    {
        //        PlantCode = x.PlantCode,
        //        CourseCode = x.CourseCode,
        //        Title = x.Title
        //    });
        //}

        // Helper untuk Auto Generate Group Code (Dari VB)
        public Task<string> GenerateGroupCodeAsync(string courseCode)
        {
            return repo.GenerateGroupCodeAsync(courseCode);
        }

        // Method pembantu (helper) agar kode pembuatan entity tidak berulang
        private static MSEquipmentGroup MapToEntity(MSEquipmentGroup model, string userId)
        {
            return new MSEquipmentGroup
            {
                GroupCode = model.GroupCode,
                BarcodeNo = model.BarcodeNo,
                Description = model.Description,
                CourseCode = model.CourseCode,
                RegisterUser = userId,
                LastUser = userId
            };
        }
    }
}
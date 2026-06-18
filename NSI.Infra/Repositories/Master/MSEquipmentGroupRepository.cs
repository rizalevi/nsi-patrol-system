using NSI.Domain.DTOs.Master;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Master;
using NSI.Infra.Datas;

namespace NSI.Infra.Repositories.Master
{
    public class MSEquipmentGroupRepository(DbExecutor dbExecutor) : IMSEquipmentGroupRepository
    {
        public async Task<IEnumerable<MSEquipmentGroupDto>> GetAllAsync(string CourseCode = "", string PlantCode = "") => await dbExecutor.QueryListAsync<MSEquipmentGroupDto>("sp_Web_MSEquipmentGroup_List", new { CourseCode, PlantCode });
        //{
        //    string sp = "sp_Web_MSEquipmentGroup_List";
        //    return await dbExecutor.QueryListAsync<MSEquipmentGroupDto>(sp, new { CourseCode, PlantCode });
        //}

        public async Task<MSEquipmentGroup?> GetDataAsync(string GroupCode) => await dbExecutor.QueryFirstOrDefaultAsync<MSEquipmentGroup>("sp_Web_MSEquipmentGroup_GetData", new { GroupCode });
        //{
        //    // Harus dibuatkan SP-nya di database (SELECT * FROM MST_Group WHERE Group_Code = @GroupCode)
        //    string sp = "sp_Web_MSEquipmentGroup_GetData";
        //    return await dbExecutor.QueryFirstOrDefaultAsync<MSEquipmentGroup>(sp, new { GroupCode });
        //}

        // Gabung Insert dan Update sesuai SP VB: "sp_MstGroup_InsertUpdate"
        public async Task CreateAsync(MSEquipmentGroup model, string userId)
        {
            string sql = @"sp_Web_MSEquipmentGroup_InsertUpdate"; // <--- Sesuai VB

            await dbExecutor.ExecuteAsync(sql, new
            {
                GroupCode = await GenerateGroupCodeAsync(model.CourseCode),
                Description = model.Description,
                CourseCode = model.CourseCode,
                RegisterUser = userId,
                LastUser = userId
            });
        }

        // Karena InsertUpdate di VB jadi satu, method Update ini memanggil SP yang sama
        public async Task UpdateAsync(MSEquipmentGroup model, string userId)
        {
            string sql = @"sp_Web_MSEquipmentGroup_InsertUpdate"; // <--- Sesuai VB
            await dbExecutor.ExecuteAsync(sql, new
            {
                GroupCode = model.GroupCode,
                Description = model.Description,
                CourseCode = model.CourseCode,
                RegisterUser = userId,
                LastUser = userId
            });
        }

        public async Task RemoveAsync(string groupCode, string courseCode, string userId, string ipAddress)
        {
            string sql = "sp_Web_MSEquipmentGroup_Delete"; // <--- Sesuai VB
            await dbExecutor.ExecuteAsync(sql, new
            {
                Group_Code = groupCode,
                Course_Code = courseCode,
                UserID = userId,
                IPAddress=ipAddress
            });
        }

        // --- Combo Box ---

        public async Task<IEnumerable<MSEquipmentGroupFilterClusterDto>> GetFilterClusterAsync() => await dbExecutor.QueryListAsync<MSEquipmentGroupFilterClusterDto>("sp_MSEquipmentGroup_Combo", new { Type = "Cluster" });
        //{
        //    // Di VB ini pakai Raw Query: SELECT Cluster_Code, Description FROM MST_Cluster
        //    // Anda harus buat SP-nya, misal: sp_MstCluster_Combo
        //    string sp = "sp_MSEquipmentGroup_Combo";
        //    return await dbExecutor.QueryListAsync<MSEquipmentGroupFilterClusterDto>(sp, new { Type = "Cluster" });
        //}

        public async Task<IEnumerable<MSEquipmentGroupFilterPlantDto>> GetFilterPlantAsync(string ClusterCode) => await dbExecutor.QueryListAsync<MSEquipmentGroupFilterPlantDto>("sp_MSEquipmentGroup_Combo", new { Type = "Plant", ClusterCode });
        //{
        //    // Di VB pakai Raw Query ke MST_Plant. Buatkan SP-nya: sp_MstPlant_Combo
        //    string sp = "sp_MSEquipmentGroup_Combo";
        //    return await dbExecutor.QueryListAsync<MSEquipmentGroupFilterPlantDto>(sp, new { Type = "Plant", ClusterCode });
        //}

        public async Task<IEnumerable<MSEquipmentGroupFilterCourseDto>> GetFilterCourseAsync(string PlantCode) => await dbExecutor.QueryListAsync<MSEquipmentGroupFilterCourseDto>("sp_MSEquipmentGroup_Combo", new { Type = "Course", PlantCode, All = "NO" });
        //{
        //    string sp = "sp_MSEquipmentGroup_Combo"; // <--- Sesuai VB
        //    return await dbExecutor.QueryListAsync<MSEquipmentGroupFilterCourseDto>(sp, new { Type="Course", PlantCode, All = "NO" });
        //}

        public async Task<string> GenerateGroupCodeAsync(string CourseCode) => (await dbExecutor.QueryFirstOrDefaultAsync<string>("sp_AutoGenerateGroupCode", new { CourseCode })) ?? string.Empty;
        //{
        //    string sp = "sp_AutoGenerateGroupCode"; // <--- Sesuai VB
        //    var result = await dbExecutor.QueryFirstOrDefaultAsync<dynamic>(sp, new { CourseCode });
        //    return result?.Group_Code?.ToString() ?? string.Empty;
        //}
    }
}
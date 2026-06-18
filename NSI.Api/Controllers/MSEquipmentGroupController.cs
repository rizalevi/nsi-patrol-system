using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Api.Services.Master; // Asumsi tempat MSGroupRepositories berada
using NSI.Domain.DTOs.Master;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Master;
using System.Security.Claims;

namespace NSI.Api.Controllers.Master
{
    [Authorize]
    [ApiController]
    [Route("api/msequipmentgroup")] // Path disesuaikan untuk Group
    public class MSEquipmentGroupController : CommonController // Mewarisi Success() dari CommonController
    {
        private readonly IMSEquipmentGroupRepository _repo;
        public MSEquipmentGroupController(IMSEquipmentGroupRepository repo)
        {
            _repo = repo;
        }


        [HttpPost("list")]
        public async Task<IActionResult> ListData([FromBody] MSGroupParameterDto payload) => Success(await _repo.GetAllAsync(payload.CourseCode, payload.PlantCode));

        [HttpPost("detail")]
        public async Task<IActionResult> GetData([FromBody] MSEquipmentGroup payload) => Success(await _repo.GetDataAsync(payload.GroupCode));

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] MSEquipmentGroup payload)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            await _repo.CreateAsync(payload, userId);

            return Success(null, "Data saved successfully");
        }

        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromBody] MSEquipmentGroup payload)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            await _repo.UpdateAsync(payload, userId);

            return Success(null, "Data updated successfully");
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] MSEquipmentGroup payload)
        {
            //string? clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            // Melempar GroupCode dan CourseCode karena di SP Remove-nya membutuhkan itu
            await _repo.RemoveAsync(payload.GroupCode, payload.CourseCode, userId, payload.IPAddress ?? "");

            return Success(null, "Data deleted successfully");
        }

        // --- Endpoints Khusus untuk Combo Box / Dropdown ---

        [HttpPost("filter-cluster")]
        public async Task<IActionResult> GetFilterCluster() => Success(await _repo.GetFilterClusterAsync());

        [HttpPost("filter-plant")]
        public async Task<IActionResult> GetFilterPlant([FromBody] MSClusterParameterDto payload) => Success(await _repo.GetFilterPlantAsync(payload.ClusterCode));

        [HttpPost("filter-course")]
        public async Task<IActionResult> GetFilterCourse([FromBody] MSPlantParameterDto payload) => Success(await _repo.GetFilterCourseAsync(payload.PlantCode));

        [HttpPost("generate-code")]
        public async Task<IActionResult> GenerateCode([FromBody] MSGroupParameterDto payload) => Success(await _repo.GenerateGroupCodeAsync(payload.CourseCode));
    }
}
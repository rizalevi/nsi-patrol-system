using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Api.Services.Master; // Mengacu pada lokasi repository Anda
using NSI.Domain.Models.Master;
using System.Security.Claims;

namespace NSI.Api.Controllers.Master
{
    [Authorize]
    [ApiController]
    [Route("api/mscourse")]
    public class MSCourseController(MSCourseRepositories service) : CommonController
    {

        // Helper property untuk User ID
        private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        [HttpPost("list")]
        public async Task<IActionResult> ListData([FromBody] MSCourse payload) => Success(await service.GetAllAsync(payload.ClusterCode, payload.PlantCode));

        [HttpPost("detail")]
        public async Task<IActionResult> GetData([FromBody] MSCourse payload)
        {
            if (string.IsNullOrEmpty(payload.CourseCode))
            {
                return BadRequest("Course Code are required.");
            }

            var result = await service.GetDataAsync(payload.ClusterCode, payload.PlantCode, payload.CourseCode);
            return Success(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] MSCourse payload)
        {
            // Validasi sederhana sebelum memanggil service
            if (string.IsNullOrEmpty(payload.ClusterCode) || string.IsNullOrEmpty(payload.PlantCode))
            {
                return BadRequest("Cluster Code and Plant Code are required.");
            }

            await service.CreateAsync(payload, CurrentUserId);
            return Success(null, "Data saved successfully");
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] MSCourse payload)
        {
            if (string.IsNullOrEmpty(payload.ClusterCode) || string.IsNullOrEmpty(payload.PlantCode))
            {
                return BadRequest("Cluster Code and Plant Code are required.");
            }

            await service.UpdateAsync(payload, CurrentUserId);
            return Success(null, "Data updated successfully");
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] MSCourse payload)
        {
            //string clientIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
            //                  ?? HttpContext.Connection.RemoteIpAddress?.ToString()
            //                  ?? string.Empty;
           // string? clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";

            await service.RemoveAsync(payload.ClusterCode, payload.PlantCode, payload.CourseCode, CurrentUserId, payload.IPAddress);
            return Success(null, "Data deleted successfully");
        }

        // --- FILTER SECTION ---

        [HttpPost("filtercluster")]
        public async Task<IActionResult> FilterCluster() => Success(await service.GetFilterClusterAsync());

        [HttpPost("filterplant")]
        public async Task<IActionResult> FilterPlant([FromBody] MSCourse payload) => Success(await service.GetFilterPlantAsync(payload.ClusterCode));

        [HttpPost("filterfrequency")]
        public async Task<IActionResult> FilterFrequency() => Success(await service.GetFilterFrequencyAsync());
    
    }
}
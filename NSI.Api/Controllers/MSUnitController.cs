using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Api.Services.Master; // Tambahkan namespace ke Service layer Anda
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Master;
using System.Security.Claims;

namespace NSI.Api.Controllers.Master
{
    [Authorize]
    [ApiController]
    [Route("api/msunitclassification")]
    public class MSUnitClassificationController : CommonController
    {
        // 1. UBAH DI SINI: Gunakan class Service/Repositories, bukan Interface Repo langsung
       // private readonly MSUnitClassificationRepositories _service;
        private readonly IMSUnitClassificationRepository _repo;

        public MSUnitClassificationController(IMSUnitClassificationRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("list")]
        public async Task<IActionResult> ListData([FromBody] MSUnitClassification payload) => Success(await _repo.GetAll());

        [HttpPost("detail")]
        public async Task<IActionResult> GetData([FromBody] MSUnitClassification payload)
        {
            if (string.IsNullOrEmpty(payload.UnitCode))
            {
                return BadRequest("Unit Code is required.");
            }

            return Success(await _repo.GetData(payload.UnitCode));
        }

        // --- LOGIKA "SAVE" (UPSERT) ---
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] MSUnitClassification payload)
        {
            if (string.IsNullOrEmpty(payload.UnitCode))
            {
                return BadRequest("Unit Code is required.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            // 1. Cek data via service
            var existingData = await _repo.GetData(payload.UnitCode);

            // 2. Eksekusi Insert atau Update via service
            if (existingData == null)
            {
                // New
                await _repo.Create(payload, userId);
                return Success(null, "Data saved successfully");
            }
            else
            {
                // Update
                await _repo.Update(payload, userId);
                return Success(null, "Data updated successfully");
            }
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] MSUnitClassification payload)
        {
            if (string.IsNullOrEmpty(payload.UnitCode))
            {
                return BadRequest("Unit Code is required.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            //string? clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";

            // Panggil _service
            await _repo.Remove(payload.UnitCode, userId, payload.IPAddress??"");

            return Success(null, "Data deleted successfully");
        }
    }
}
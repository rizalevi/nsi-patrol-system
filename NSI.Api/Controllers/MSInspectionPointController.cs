using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Master;
using System.Security.Claims;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/msinspectionpoint")]
    public class MSInspectionPointController : CommonController
    {
        //private readonly DataLogger _logger;
        private readonly IMSInspectionPointRepository _repo;


        public MSInspectionPointController(IMSInspectionPointRepository repo)
        {
            _repo = repo;
        }


        [HttpPost("list")]
        public async Task<IActionResult> GetList([FromBody] MSInpectionPointParam payload)
        {
            var results = await _repo.GetList(payload);
            return Success(results);
        }

        #region Combo Fill


        [HttpPost("charts")]
        public async Task<IActionResult> GetCharts()
        {
            var results = await _repo.GetCharts();
            return Success(results);
        }

        [HttpPost("units")]
        public async Task<IActionResult> GetUnits()
        {
            var results = await _repo.GetUnits();
            return Success(results);
        }

        #endregion

        [HttpPost("detail")]
        public async Task<IActionResult> GetData([FromBody] MSInpectionPointDetailParam payload)
        {
            var result = await _repo.GetData(payload);
            return Success(result);
        }


        [HttpPost("getmethode")]
        public async Task<IActionResult> GetMethode([FromBody] MSInspectionPoint payload)
        {
            var results = await _repo.GetMethode(payload.UnitCode);
            return Success(results);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] MSInspectionPoint payload)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            await _repo.Create(payload, userId);

            return Success(null, "Data saved successfully");
        }

        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromBody] MSInspectionPoint payload)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            await _repo.Update(payload, userId);

            return Success(null, "Data updated successfully");
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] MSInspectionPoint payload)
        {
            string? clientIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                ?? HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            await _repo.Remove(payload.CourseCode, payload.GroupCode, payload.InspectionCode, userId,clientIp);

            return Success(null, "Data deleted successfully");
        }
    }
}

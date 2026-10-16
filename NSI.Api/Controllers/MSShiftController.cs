using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Api.Services.Master;
using NSI.Domain.DTOs.Master;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Master;
using System.Data.SqlClient;
using System.Security.Claims;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/msshift")]
    public class MSShiftController : CommonController
    {
        private IMSShiftRepository _repo;

        public MSShiftController(IMSShiftRepository repo)
        {
            _repo = repo;
        }


        [HttpPost("list")]
        public async Task<IActionResult> GetList()
        {
            var results = await _repo.GetList();
            return Success(results);
        }

        [HttpPost("detail")]
        public async Task<IActionResult> GetData([FromBody] MSShift payload)
        {
            var result = await _repo.GetData(payload.Shift_Cls);
            return Success(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] MSShiftDto payload)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
                await _repo.Create(payload, userId);

                return Success(null, "Data saved successfully");
            }
            catch (SqlException ex)
            {
                return Fail(null, ex.Message);
            }
            catch (Exception ex)
            {
                return Fail(null, ex.Message);
            }
        }

        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromBody] MSShiftDto payload)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
                await _repo.Update(payload, userId);

                return Success(null, "Data updated successfully");
            }
            catch (SqlException ex)
            {
                return Fail(null, ex.Message);
            }
            catch (Exception ex)
            {
                return Fail(null, ex.Message);
            }
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] MSShift payload)
        {
            try
            {
                //string? clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
                await _repo.Remove(payload.Shift_Cls, userId, payload.IPAddress ?? "");

                return Success(null, "Data deleted successfully");
            }
            catch (SqlException ex)
            {
                return Fail(null, ex.Message);
            }
            catch (Exception ex)
            {
                return Fail(null, ex.Message);
            }
        }
    }
}

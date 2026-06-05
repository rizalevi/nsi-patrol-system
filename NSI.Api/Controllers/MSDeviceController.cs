using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.DTOs.Master;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Inspection;
using NSI.Domain.Models.Master;
using System.Security.Claims;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/msdevice")]
    public class MSDeviceController : CommonController
    {
        //private readonly DataLogger _logger;
        private readonly IMSDeviceRepository _repo;


        public MSDeviceController(IMSDeviceRepository repo)
        {
            _repo = repo;
        }

        #region MS device
        [HttpPost("list")]
        public async Task<IActionResult> GetList()
        {
            var results = await _repo.GetList();
            return Success(results);
        }

        [HttpPost("detail")]
        public async Task<IActionResult> GetData([FromBody] MSDeviceDto payload)
        {
            var result = await _repo.GetData(payload.DeviceCode);
            return Success(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] MSDevice payload)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            await _repo.Create(payload, userId);

            return Success(null, "Data saved successfully");
        }

        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromBody] MSDevice payload)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            await _repo.Update(payload, userId);

            return Success(null, "Data updated successfully");
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] MSDevice payload)
        {
            string? clientIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                ?? HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            await _repo.Remove(payload.DeviceCode, userId, clientIp);

            return Success(null, "Data deleted successfully");
        }

        [HttpPost("get-plant")]
        public async Task<IActionResult> GetPlants()
        {
            var results = await _repo.GetPlants();
            return Success(results);
        }
        #endregion

        #region MS device detail
        [HttpPost("get-device")]
        public async Task<IActionResult> GetDeviceMaster()
        {
            var results = await _repo.GetDeviceMaster();
            return Success(results);
        }

        [HttpPost("list-detail")]
        public async Task<IActionResult> GetListDetail([FromBody] MSDeviceDetailDto payload)
        {
            var results = await _repo.GetListDetail(payload.DeviceCode, payload.PlantCode);
            return Success(results);
        } 

        [HttpPost("submit-detail")]
        public async Task<IActionResult> CreateHTDetail([FromBody] MSDeviceBatch payload)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            await _repo.SubmitBatch(payload, userId);

            return Success(null, "Data submit successfully");
        }

       

        //[HttpDelete("removeHTDetail")]
        //public async Task<IActionResult> RemoveHTDetail([FromBody] MSHTDetail payload)
        //{
        //    string? clientIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
        //        ?? HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";

        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        //    await _repo.RemoveHTDetail(payload.HT_Code, payload.Plant_Code, payload.Course_Code, userId);

        //    return Success(null, "Data deleted successfully");
        //}

        //[HttpPost("plantsHTDetail")]
        //public async Task<IActionResult> GetPlantsHTDetail([FromBody] MSHTDetail payload)
        //{
        //    var results = await _repo.GetPlantsHTDetail();
        //    return Success(results);
        //}

        #endregion
    }
}

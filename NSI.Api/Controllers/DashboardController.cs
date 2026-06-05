using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.DTOs.Dashboard;
using NSI.Domain.Interfaces.Dashboard;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : CommonController
    {
        //private readonly DataLogger _logger;
        private readonly IDashboardRepository _repo;

        public DashboardController(IDashboardRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("overview")]
        public async Task<IActionResult> GetDashboardOverview([FromBody] DashboardParamDto model)
        {
            try
            {
                var result = await _repo.GetDashboardOverview(model);
                return Success(result);
                //new ContentResult
                //{
                //    StatusCode = 200,
                //    ContentType = "application/json",
                //    Content = System.Text.Json.JsonSerializer.Serialize(result)
                //};
            }
            catch (Exception ex)
            {
                // Log the exception (you can use your preferred logging framework)
                // _logger.LogError(ex, "Failed to get dashboard overview");
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        //list progress detail
        [HttpPost("progresslist-detail")]
        public async Task<IActionResult> ListDetail([FromBody] ViewDetailProgressParamDto model)
        {
            var result = await _repo.ListRemainingData(model);
            return Success(result);

        }



        //list detail abnormal data realtime shift
        [HttpPost("realtimeshift-detail-abnormal")]
        public async Task<IActionResult> ListAbnormalRealtimeShift([FromBody] DashboardParamDto model)
        {
            var result = await _repo.ListAbnormalRealtimeShift(model);
            return Success(result);

        }

    }
}

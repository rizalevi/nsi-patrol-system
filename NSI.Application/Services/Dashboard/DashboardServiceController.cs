using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Extensions;
using NSI.Domain.DTOs.Dashboard;
using NSI.Domain.DTOs.Inspection;
using NSI.Infra.Helpers;
using System.Security.Claims;
using System.Text.Json;

namespace NSI.Application.Services.Dashboard
{


    [ApiController, Route("service/dashboard")]
    public class DashboardServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;

        public DashboardServiceController(HttpRequestHelper htr)
        {
            _htr = htr;
        }

        [HttpPost("overview")]
        public async Task<IActionResult> GetDashboardOverview()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            DashboardParamDto model = new DashboardParamDto();
            model.UserId = userId;
            //var result = await _htr.PostAsyncDataTable<List>DashboardDto>>(JsonSerializer.Serialize(model), "api/dashboard/overview");

            var result = await _htr.PostAsyncDataTable<DashboardDto>(
            JsonSerializer.Serialize(model),
            "api/dashboard/overview"
            );
            return this.ResponseStatus(result);
            //return Ok(result);


        }


        [Authorize, HttpPost, Route("list-detail")]
        public async Task<IActionResult> GetList([FromBody] ViewDetailProgressParamDto model)
        {
            //ResponseModel result = await _htr.AsyncAsResponseModel(JsonSerializer.Serialize(param), "ApiClient", "/api/dailyactivity/getdetailformula");
            model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _htr.PostAsyncDataTable<List<ProgressInspectionDetailDto>>(
                 JsonSerializer.Serialize(model),
                "api/dashboard/progresslist-detail"
            );
            return this.ResponseStatus(result);
            //return Json(new
            //{
            //    status = result.Status,
            //    message = result.Message,
            //    data = result.Data
            //});
        }

        [Authorize, HttpPost, Route("realtimeshift-detail-abnormal")]
        public async Task<IActionResult> GetDetailAbnormal()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            DashboardParamDto model = new DashboardParamDto();
            model.UserId = userId;

            var result = await _htr.PostAsyncDataTable<List<RealtimeShiftDetailAbnormalDto>>(
                 JsonSerializer.Serialize(model),
                "api/dashboard/realtimeshift-detail-abnormal"
            );
            return this.ResponseStatus(result);
          

        }
       

    }
}

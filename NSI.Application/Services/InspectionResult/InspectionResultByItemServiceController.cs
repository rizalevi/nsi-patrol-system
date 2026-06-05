using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Extensions;
using NSI.Domain.DTOs.InspectionResult;
using NSI.Domain.DTOs.Tasklist;
using NSI.Infra.Helpers;
using System.Security.Claims;
using System.Text.Json;

namespace NSI.Application.Services.InspectionResult
{


    [ApiController, Route("service/inspectionresultbyitem")]
    public class InspectionResultByItemServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;

        public InspectionResultByItemServiceController(HttpRequestHelper htr)  
        {
            _htr = htr;
        }

        [Authorize, HttpPost, Route("get-userplant")]
        public async Task<IActionResult> GetUserGroup()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncStringList<List<UserPlantDto>>(
                userId,
                "api/inspectionresultbyitem/get-userplant",
                HttpMethod.Post

            );
            return this.ResponseStatus(result);
            //return Json(new
            //{
            //    status = result.Status,
            //    message = result.Message,
            //    data = result.Data
            //});
        }


        [Authorize, HttpPost, Route("list")]
        public async Task<IActionResult> GetList([FromBody] InspectionResultByItemParamDto payload)
        {
            //ResponseModel result = await _htr.AsyncAsResponseModel(JsonSerializer.Serialize(param), "ApiClient", "/api/dailyactivity/getdetailformula");
            var result = await _htr.PostAsyncDataTable<List<InspectionResultSummaryByItemDto>>(
                 JsonSerializer.Serialize(payload),
                "api/inspectionresultbyitem/get-data"
            );
            return this.ResponseStatus(result);
            //return Json(new
            //{
            //    status = result.Status,
            //    message = result.Message,
            //    data = result.Data
            //});
        }



    }
}

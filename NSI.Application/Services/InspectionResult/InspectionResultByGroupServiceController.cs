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


    [ApiController, Route("service/inspectionresultbygroup")]
    public class InspectionResultByGroupServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;

        public InspectionResultByGroupServiceController(HttpRequestHelper htr)  
        {
            _htr = htr;
        }

      
        [Authorize, HttpPost, Route("list")]
        public async Task<IActionResult> GetList([FromBody] InspectionResultByGroupParamDto payload)
        {
            //ResponseModel result = await _htr.AsyncAsResponseModel(JsonSerializer.Serialize(param), "ApiClient", "/api/dailyactivity/getdetailformula");
            var result = await _htr.PostAsyncDataTable<List<InspectionResultSummaryByGroupDto>>(
                 JsonSerializer.Serialize(payload),
                "api/inspectionresultbygroup/get-data"
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

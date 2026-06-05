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


    [ApiController, Route("service/inspectionresultanalysis")]
    public class InspectionResultAnalysisServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;

        public InspectionResultAnalysisServiceController(HttpRequestHelper htr)  
        {
            _htr = htr;
        }

        [Authorize, HttpPost, Route("get-userplant")]
        public async Task<IActionResult> GetUserGroup()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncStringList<List<UserPlantDto>>(
                userId,
                "api/inspectionresultanalysis/get-userplant",
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
        public async Task<IActionResult> GetList([FromBody] InspectionResultParamDto payload)
        {
            //ResponseModel result = await _htr.AsyncAsResponseModel(JsonSerializer.Serialize(param), "ApiClient", "/api/dailyactivity/getdetailformula");
            var result = await _htr.PostAsyncDataTable<List<InspectionResultAnalysisDto>>(
                 JsonSerializer.Serialize(payload),
                "api/inspectionresultanalysis/get-data"
            );
            return this.ResponseStatus(result);
            //return Json(new
            //{
            //    status = result.Status,
            //    message = result.Message,
            //    data = result.Data
            //});
        }

        [Authorize, HttpPost, Route("chart")]
        public async Task<IActionResult> GetChart([FromBody] InspectionResultParamDto payload)
        {
            var result = await _htr.PostAsyncDataTable<List<InspectionResultAnalysisChartDto>>(
                 JsonSerializer.Serialize(payload),
                "api/inspectionresultanalysis/get-chart"
            );
            return this.ResponseStatus(result);
            //return Json(new
            //{
            //    status = result.Status,
            //    message = result.Message,
            //    data = result.Data
            //});
        }

        [Authorize, HttpPost , Route("get-unitname")]
        public async Task<IActionResult> GetUnit([FromBody] InspectionResultParamDto payload)
        {
            var result = await _htr.PostAsyncDataTable<List<InspectionGetUnitDto>>(
                 JsonSerializer.Serialize(payload),
                "api/inspectionresultanalysis/get-unitname"
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

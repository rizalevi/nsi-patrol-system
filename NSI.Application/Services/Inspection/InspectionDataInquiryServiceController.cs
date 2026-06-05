using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Extensions;
using NSI.Domain.DTOs.Inspection;
using NSI.Infra.Helpers;
using System.Text.Json;

namespace NSI.Application.Services.Inspection
{


    [ApiController, Route("service/inspectiondatainquiry")]
    public class InspectionDataInquiryServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;

        public InspectionDataInquiryServiceController(HttpRequestHelper htr)  
        {
            _htr = htr;
        }

       

        [Authorize, HttpPost, Route("list")]
        public async Task<IActionResult> GetList([FromBody] InspectionDataInquiryParamDto payload)
        {
            //ResponseModel result = await _htr.AsyncAsResponseModel(JsonSerializer.Serialize(param), "ApiClient", "/api/dailyactivity/getdetailformula");
            var result = await _htr.PostAsyncDataTable<List<InspectionDataInquiryListDto>>(
                 JsonSerializer.Serialize(payload),
                "api/inspectiondatainquiry/list"
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

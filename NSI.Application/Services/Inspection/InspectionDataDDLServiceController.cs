using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Extensions;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.DTOs.Security;
using NSI.Infra.Helpers;
using System.Text.Json;

namespace NSI.Application.Services.Inspection
{


    [ApiController, Route("service/inspectiondata")]
    public class InspectionDataDDLServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;

        public InspectionDataDDLServiceController(HttpRequestHelper htr)  
        {
            _htr = htr;
        }

        [Authorize, HttpPost, Route("get-cluster")]
        public async Task<IActionResult> GetCluster()
        {
            var result = await _htr.PostAsyncDataTable<List<InspectionDataDDLDto>>(
                "{}",
                "api/inspectiondata/cluster-ddl"
            );

            return this.ResponseStatus(result);

            //return Json(new {
            //    status = result.Status,
            //    message = result.Message,
            //    data = result.Data
            //});
        }

        [Authorize, HttpPost, Route("get-plant")]
        public async Task<IActionResult> GetPlant([FromBody] InspectionDataInquiryParamDto payload)
        {
            var result = await _htr.PostAsyncDataTable<List<InspectionDataDDLDto>>(
                 JsonSerializer.Serialize(payload),
                "api/inspectiondata/plant-ddl"
            );
            return this.ResponseStatus(result);
            //return Json(new
            //{
            //    status = result.Status,
            //    message = result.Message,
            //    data = result.Data
            //});
        }

        [Authorize, HttpPost, Route("get-course")]
        public async Task<IActionResult> GetCourse([FromBody] InspectionDataInquiryParamDto payload)
        {
            var result = await _htr.PostAsyncDataTable<List<InspectionDataDDLDto>>(
                 JsonSerializer.Serialize(payload),
                "api/inspectiondata/course-ddl"
            );
            return this.ResponseStatus(result);
            //return Json(new
            //{
            //    status = result.Status,
            //    message = result.Message,
            //    data = result.Data
            //});
        }

        [Authorize, HttpPost, Route("get-group")]
        public async Task<IActionResult> GetGroup([FromBody] InspectionDataInquiryParamDto payload)
        {
            var result = await _htr.PostAsyncDataTable<List<InspectionDataDDLDto>>(
                 JsonSerializer.Serialize(payload),
                "api/inspectiondata/group-ddl"
            );
            return this.ResponseStatus(result);
            //return Json(new
            //{
            //    status = result.Status,
            //    message = result.Message,
            //    data = result.Data
            //});
        }

        [Authorize, HttpPost, Route("get-inspectionpoint")]
        public async Task<IActionResult> GetInspectionPoint([FromBody] InspectionDataInquiryParamDto payload)
        {
            var result = await _htr.PostAsyncDataTable<List<InspectionDataDDLDto>>(
                 JsonSerializer.Serialize(payload),
                "api/inspectiondata/inspectionpoint-ddl"
            );
            return this.ResponseStatus(result);
            //return Json(new
            //{
            //    status = result.Status,
            //    message = result.Message,
            //    data = result.Data
            //});
        }

        [Authorize, HttpPost, Route("get-frequency")]
        public async Task<IActionResult> GetShift()
        {
            var result = await _htr.PostAsyncDataTable<List<InspectionDataDDLDto>>(
                "{}",
                "api/inspectiondata/frequency-ddl"
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
        public async Task<IActionResult> GetList([FromBody] InspectionDataInquiryParamDto payload)
        {
            //ResponseModel result = await _htr.AsyncAsResponseModel(JsonSerializer.Serialize(param), "ApiClient", "/api/dailyactivity/getdetailformula");
            var result = await _htr.PostAsyncDataTable<List<UserSetupDto>>(
                 JsonSerializer.Serialize(payload),
                "api/inspectiondatainquiry/list"
            );
            return this.ResponseStatus(result);
            //return Json(new {
            //    status = result.Status,
            //    message = result.Message,
            //    data = result.Data });
        }

        /*dipakai untuk inspection resulyt analysis*/
        [Authorize, HttpPost, Route("get-inspectionpoint-chart")]
        public async Task<IActionResult> GetInspectionPointForChart([FromBody] InspectionDataInquiryParamDto payload)
        {
            var result = await _htr.PostAsyncDataTable<List<InspectionDataDDLDto>>(
                 JsonSerializer.Serialize(payload),
                "api/inspectiondata/inspectionpoint-chart-ddl"
            );
            return this.ResponseStatus(result);
            //return Json(new
            //{
            //    status = result.Status,
            //    message = result.Message,
            //    data = result.Data
            //});
        }
        [Authorize, HttpPost, Route("get-unit")]
        public async Task<IActionResult> GetUnit()
        {
            var result = await _htr.PostAsyncDataTable<List<InspectionDataUnitDDLDto>>(
                "{}",
                "api/inspectiondata/unit-ddl"
            );

            return Json(new
            {
                status = result.Status,
                message = result.Message,
                data = result.Data
            });
        }

        [Authorize, HttpPost, Route("get-unitoption")]
        public async Task<IActionResult> GetUnitOption([FromBody] InspectionDataUploadListDto payload)
        {
            var result = await _htr.PostAsyncDataTable<List<InspectionDataUnitOptionDDLDto>>(
                JsonSerializer.Serialize(payload),
                "api/inspectiondata/unitoption-ddl"
            );

            return Json(new
            {
                status = result.Status,
                message = result.Message,
                data = result.Data
            });
        }

    }
}

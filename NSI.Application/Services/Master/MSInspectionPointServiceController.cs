using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Extensions;
using NSI.Domain.DTOs.Master;
using NSI.Domain.Models.Master;
using NSI.Infra.Helpers;
using System.Security.Claims;
using System.Text.Json;

namespace NSI.Application.Services.Master
{
    [ApiController, Route("service/msinspectionpoint")]
    public class MSInspectionPointServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;

        public MSInspectionPointServiceController(HttpRequestHelper htr)
        {
            _htr = htr;
        }

        [Authorize, HttpPost, Route("list")]
        public async Task<IActionResult> GetList([FromBody] MSInpectionPointParam model)
        {
            var result = await _htr.PostAsyncDataTable<List<MSInspectionPointDto>>(
               JsonSerializer.Serialize(model),
                "api/msinspectionpoint/list"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }

        #region combo

        [Authorize, HttpPost, Route("charts")]
        public async Task<IActionResult> GetCharts()
        {
            var result = await _htr.PostAsyncDataTable<List<MSInspectionPointDDLDto>>(
                "{}",
                "api/msinspectionpoint/charts"
            );
            return this.ResponseStatus(result);
           
            //return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("units")]
        public async Task<IActionResult> GetUnits()
        {
            var result = await _htr.PostAsyncDataTable<List<MSInspectionPointUnitDDLDto>>(
                "{}",
                "api/msinspectionpoint/units"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }

        #endregion

        [Authorize, HttpPost, Route("detail")]
        public async Task<IActionResult> GetData([FromBody] MSInpectionPointDetailParam model)
        {
            var result = await _htr.PostAsyncDataTable<MSInspectionPointDto>(
                JsonSerializer.Serialize(model),
                "api/msinspectionpoint/detail"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });

        }

        //[Authorize, HttpPost, Route("checkinspectioncode")]
        //public async Task<IActionResult> CheckInspectionCode([FromBody] MSInspectionPointDto model)
        //{
        //    var result = await _htr.PostAsyncDataTable<MSInspectionPointDto>(
        //        JsonSerializer.Serialize(model),
        //        "api/msinspectionpoint/checkinspectioncode"
        //    );
        //    return Json(new { data = result.Data });
        //}

        //[Authorize, HttpPost, Route("generateinspectioncode")]
        //public async Task<IActionResult> GenerateInspectionCode([FromBody] MSInspectionPointDto model)
        //{
        //    var result = await _htr.PostAsyncDataTable<MSInspectionPointDto>(
        //        JsonSerializer.Serialize(model),
        //        "api/msinspectionpoint/generateinspectioncode"
        //    );
        //    return Json(new { data = result.Data });
        //}

        [Authorize, HttpPost, Route("getmethode")]
        public async Task<IActionResult> GetMethode([FromBody] MSInspectionPoint model)
        {
            var result = await _htr.PostAsyncDataTable<List<MSInspectionPointDDLDto>>(
                JsonSerializer.Serialize(model),
                "api/msinspectionpoint/getmethode"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("create")]
        public async Task<IActionResult> Create([FromBody] MSInspectionPoint model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(model),
                "api/msinspectionpoint/create",
                HttpMethod.Post
            );
            return this.ResponseStatus(result);
        }

        [Authorize, HttpPatch, Route("update")]
        public async Task<IActionResult> Update([FromBody] MSInspectionPoint model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(model),
                "api/msinspectionpoint/update",
                HttpMethod.Patch
            );
            return this.ResponseStatus(result);
        }

        //delete
        [Authorize, HttpPost, Route("delete")]
        public async Task<IActionResult> Delete([FromBody] MSInspectionPoint model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            model.IPAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";

            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(model),
                "api/msinspectionpoint/remove",
                HttpMethod.Post
            );
            return this.ResponseStatus(result);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Extensions;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.DTOs.Master;
using NSI.Domain.Models.Master;
using NSI.Infra.Helpers;
using System.Security.Claims;
using System.Text.Json;

namespace NSI.Application.Services.Master
{
    [ApiController, Route("service/msdevice")]
    public class MSDeviceServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;

        public MSDeviceServiceController(HttpRequestHelper htr)
        {
            _htr = htr;
        }

        #region MS device
        [Authorize, HttpPost, Route("list")]
        public async Task<IActionResult> GetList()
        {
            var result = await _htr.PostAsyncDataTable<List<MSDeviceDto>>(
               "{}",
                "api/msdevice/list"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("detail")]
        public async Task<IActionResult> GetData([FromBody] MSDeviceDto model)
        {
            var result = await _htr.PostAsyncDataTable<MSDeviceDto>(
                JsonSerializer.Serialize(model),
                "api/msdevice/detail"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });

        }

        [Authorize, HttpPost, Route("create")]
        public async Task<IActionResult> Create([FromBody] MSDevice model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(model),
                "api/msdevice/create",
                HttpMethod.Post
            );
            return this.ResponseStatus(result);
        }

        [Authorize, HttpPatch, Route("update")]
        public async Task<IActionResult> Update([FromBody] MSDevice model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(model),
                "api/msdevice/update",
                HttpMethod.Patch
            );
            return this.ResponseStatus(result);
        }

        //delete
        [Authorize, HttpPost, Route("delete")]
        public async Task<IActionResult> Delete([FromBody] MSDevice model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(model),
                "api/msdevice/remove",
                HttpMethod.Post
            );
            return this.ResponseStatus(result);
        }

        [Authorize, HttpPost, Route("get-plant")]
        public async Task<IActionResult> GetPlants()
        {
            var result = await _htr.PostAsyncDataTable<object>(
                "{}",
                "api/msdevice/get-plant"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }
        #endregion

        #region MS device detail
        [Authorize, HttpPost, Route("get-listdevice")]
        public async Task<IActionResult> GetListDeviceData()
        {
            var result = await _htr.PostAsyncDataTable<List<MSDeviceDDLDto>>(
                "{}",
                "api/msdevice/get-device"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("list-detail")]
        public async Task<IActionResult> GetListDetail([FromBody] MSDeviceDetailDto model)
        {
            var result = await _htr.PostAsyncDataTable<List<MSDeviceDetailDto>>(
               JsonSerializer.Serialize(model),
                "api/msdevice/list-detail"
            );

            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }


        [Authorize, HttpPost, Route("submit-batch")]
        public async Task<IActionResult> SubmitDetail([FromBody] MSDeviceBatch model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(model),
                "api/msdevice/submit-detail",
                HttpMethod.Post
            );

            return this.ResponseStatus(result);
        }

       
        #endregion
    }
}

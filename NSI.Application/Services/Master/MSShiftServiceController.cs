using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Extensions;
using NSI.Domain.DTOs.Master;
using NSI.Infra.Helpers;
using System.Security.Claims;
using System.Text.Json;

namespace NSI.Application.Services.Master
{
    [ApiController, Route("service/msshift")]
    public class MSShiftServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;

        public MSShiftServiceController(HttpRequestHelper htr)
        {
            _htr = htr;
        }

        [Authorize, HttpPost, Route("list")]
        public async Task<IActionResult> GetList()
        {
            var result = await _htr.PostAsyncDataTable<List<MSShiftDto>>(
               "{}",
                "api/msshift/list"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("detail")]
        public async Task<IActionResult> GetData([FromBody] MSShiftDto model)
        {
            var result = await _htr.PostAsyncDataTable<MSShiftDto>(
                JsonSerializer.Serialize(model),
                "api/msshift/detail"
            ); 
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });

        }

        [Authorize, HttpPost, Route("create")]
        public async Task<IActionResult> Create([FromBody] MSShiftDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(model),
                "api/msshift/create",
                HttpMethod.Post
            );
            return this.ResponseStatus(result);

        }

        [Authorize, HttpPatch, Route("update")]
        public async Task<IActionResult> Update([FromBody] MSShiftDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(model),
                "api/msshift/update",
                HttpMethod.Patch
            ); return this.ResponseStatus(result);
        }

        //delete
        [Authorize, HttpPost, Route("delete")]
        public async Task<IActionResult> Delete([FromBody] MSShiftDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(model),
                "api/msshift/remove",
                HttpMethod.Post
            ); return this.ResponseStatus(result);
        }
    }
}

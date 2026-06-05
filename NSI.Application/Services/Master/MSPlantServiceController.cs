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
    [ApiController, Route("service/msplant")]
    public class MSPlantServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;

        public MSPlantServiceController(HttpRequestHelper htr)
        {
            _htr = htr;
        }

        [Authorize, HttpPost, Route("list")]
        public async Task<IActionResult> GetList([FromBody] MSClusterParameterDto model)
        {
            var result = await _htr.PostAsyncDataTable<List<MSPlantDto>>(
               JsonSerializer.Serialize(model),
                "api/msplant/list"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("detail")]
        public async Task<IActionResult> GetData([FromBody] MSPlant model)
        {
            var result = await _htr.PostAsyncDataTable<MSPlant>(
                JsonSerializer.Serialize(model),
                "api/msplant/detail"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });

        }

        [Authorize, HttpPost, Route("create")]
        public async Task<IActionResult> Create([FromBody] MSPlant model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(model),
                "api/msplant/create",
                HttpMethod.Post
            );
            return this.ResponseStatus(result);
            //return Json(new { success = result.Status, message = result.Message });

        }

        [Authorize, HttpPatch, Route("update")]
        public async Task<IActionResult> Update([FromBody] MSPlant model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(model),
                "api/msplant/update",
                HttpMethod.Patch
            );
            return this.ResponseStatus(result);
            //return Json(new { success = result.Status, message = result.Message });
        }

        //delete
        [Authorize, HttpPost, Route("delete")]
        public async Task<IActionResult> Delete([FromBody] MSPlant model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(model),
                "api/msplant/remove",
                HttpMethod.Post
            );
            return this.ResponseStatus(result);
            //return Json(new { success = result.Status, message = result.Message });
        }
    }
}

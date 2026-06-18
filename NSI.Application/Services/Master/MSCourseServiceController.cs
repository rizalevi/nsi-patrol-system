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
    [ApiController, Route("service/mscourse")]
    public class MSCourseServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;

        public MSCourseServiceController(HttpRequestHelper htr)
        {
            _htr = htr;
        }

        [Authorize, HttpPost, Route("list")]
        public async Task<IActionResult> GetList([FromBody] MSCourse payload)
        {
            var result = await _htr.PostAsyncDataTable<List<MSCourseDto>>(
                JsonSerializer.Serialize(payload),
                "api/mscourse/list"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("detail")]
        public async Task<IActionResult> GetData([FromBody] MSCourse payload)
        {
            var result = await _htr.PostAsyncDataTable<MSCourse>(
                JsonSerializer.Serialize(payload),
                "api/mscourse/detail"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("create")]
        public async Task<IActionResult> Create([FromBody] MSCourse payload)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(payload),
                "api/mscourse/create",
                HttpMethod.Post
            );

            return this.ResponseStatus(result);

        }

        [Authorize, HttpPost, Route("update")]
        public async Task<IActionResult> Update([FromBody] MSCourse payload)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(payload),
                "api/mscourse/update",
                HttpMethod.Post
            );
            return this.ResponseStatus(result);
        }

        //delete
        [Authorize, HttpPost, Route("remove")]
        public async Task<IActionResult> Remove([FromBody] MSCourse payload)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            payload.IPAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(payload),
                "api/mscourse/remove",
                HttpMethod.Post
            );
            return this.ResponseStatus(result);
        }

        [Authorize, HttpPost, Route("filtercluster")]
        public async Task<IActionResult> GetFilterCluster()
        {
            var result = await _htr.PostAsyncDataTable<List<MSCourseFilterClusterDto>>(
                "{}",
                "api/mscourse/filtercluster"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("filterplant")]
        public async Task<IActionResult> GetFilterPlant([FromBody] MSCourseFilterPlant payload)
        {
            var result = await _htr.PostAsyncDataTable<List<MSCourseFilterPlantDto>>(
                JsonSerializer.Serialize(payload),
                "api/mscourse/filterplant"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("filterfrequency")]
        public async Task<IActionResult> GetFilterStatusShift()
        {
            var result = await _htr.PostAsyncDataTable<List<MSCourseFilterFrequencyDto>>(
               "{}",
                "api/mscourse/filterfrequency"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }
    }
}

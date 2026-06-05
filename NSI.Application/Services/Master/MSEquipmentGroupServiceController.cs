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
    [ApiController, Route("service/msequipmentgroup")]
    public class MSEquipmentGroupServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;

        public MSEquipmentGroupServiceController(HttpRequestHelper htr)
        {
            _htr = htr;
        }

        // Gunakan MSGroupParameterDto (berisi CourseCode) sesuai dengan Controller API
        [Authorize, HttpPost, Route("list")]
        public async Task<IActionResult> GetList([FromBody] MSEquipmentGroupParameterDto model)
        {
            var result = await _htr.PostAsyncDataTable<List<MSEquipmentGroupDto>>(
               JsonSerializer.Serialize(model),
                "api/msequipmentgroup/list"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("detail")]
        public async Task<IActionResult> GetData([FromBody] MSEquipmentGroup model)
        {
            var result = await _htr.PostAsyncDataTable<MSEquipmentGroup>(
                JsonSerializer.Serialize(model),
                "api/msequipmentgroup/detail"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("create")]
        public async Task<IActionResult> Create([FromBody] MSEquipmentGroup model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(model),
                "api/msequipmentgroup/create",
                HttpMethod.Post
            );
            return this.ResponseStatus(result);
        }

        [Authorize, HttpPatch, Route("update")]
        public async Task<IActionResult> Update([FromBody] MSEquipmentGroup model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(model),
                "api/msequipmentgroup/update",
                HttpMethod.Patch
            );
            return this.ResponseStatus(result);
        }

        [Authorize, HttpPost, Route("delete")]
        public async Task<IActionResult> Delete([FromBody] MSEquipmentGroup model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(model),
                "api/msequipmentgroup/remove",
                HttpMethod.Post
            );
            return this.ResponseStatus(result);
        }

        // --- Endpoints Tambahan untuk Combo Box & Generate Code ---

        [Authorize, HttpPost, Route("filter-cluster")]
        public async Task<IActionResult> GetFilterCluster()
        {
            // Karena tidak butuh parameter, kirim empty object "{}"
            var result = await _htr.PostAsyncDataTable<List<MSEquipmentGroupFilterClusterDto>>(
               "{}",
               "api/msequipmentgroup/filter-cluster"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("filter-plant")]
        public async Task<IActionResult> GetFilterPlant([FromBody] MSClusterParameterDto model)
        {
            var result = await _htr.PostAsyncDataTable<List<MSEquipmentGroupFilterPlantDto>>(
               JsonSerializer.Serialize(model),
               "api/msequipmentgroup/filter-plant"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("filter-course")]
        public async Task<IActionResult> GetFilterCourse([FromBody] MSPlantParameterDto model)
        {
            var result = await _htr.PostAsyncDataTable<List<MSEquipmentGroupFilterCourseDto>>(
               JsonSerializer.Serialize(model),
               "api/msequipmentgroup/filter-course"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("generate-code")]
        public async Task<IActionResult> GenerateCode([FromBody] MSEquipmentGroupParameterDto model)
        {
            // Menangkap dynamic object atau DTO yang isinya property GroupCode
            var result = await _htr.PostAsyncDataTable<object>(
               JsonSerializer.Serialize(model),
               "api/msequipmentgroup/generate-code"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }
    }
}
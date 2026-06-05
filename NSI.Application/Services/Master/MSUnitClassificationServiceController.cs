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
    [ApiController, Route("service/msunitclassification")]
    public class MSUnitClassificationServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;

        public MSUnitClassificationServiceController(HttpRequestHelper htr)
        {
            _htr = htr;
        }

        [Authorize, HttpPost, Route("list")]
        public async Task<IActionResult> GetList([FromBody] MSUnitClassification model)
        {
            var result = await _htr.PostAsyncDataTable<List<MSUnitClassificationDto>>(
               JsonSerializer.Serialize(model),
                "api/msunitclassification/list"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("detail")]
        public async Task<IActionResult> GetData([FromBody] MSUnitClassification model)
        {
            var result = await _htr.PostAsyncDataTable<MSUnitClassification>(
                JsonSerializer.Serialize(model),
                "api/msunitclassification/detail"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }

        // --- PERUBAHAN: GABUNG CREATE DAN UPDATE MENJADI SAVE ---
        [Authorize, HttpPost, Route("save")]
        public async Task<IActionResult> Save([FromBody] MSUnitClassification model)
        {
            // Lempar payload ke API Utama endpoint /save. 
            // Logika pengecekan insert/update akan ditangani oleh API Utama.
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(model),
                "api/msunitclassification/save",
                HttpMethod.Post
            );
            return this.ResponseStatus(result);
        }

        // delete
        [Authorize, HttpPost, Route("delete")]
        public async Task<IActionResult> Delete([FromBody] MSUnitClassification model)
        {
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(model),
                "api/msunitclassification/remove",
                HttpMethod.Post
            );
            return this.ResponseStatus(result);
        }
    }
}
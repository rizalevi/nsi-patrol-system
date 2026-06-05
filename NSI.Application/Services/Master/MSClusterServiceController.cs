using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Extensions;
using NSI.Domain.DTOs;
using NSI.Domain.DTOs.Master;
using NSI.Domain.Models.Auth;
using NSI.Domain.Models.Master;
using NSI.Infra.Helpers;
using System.Security.Claims;
using System.Text.Json;

namespace NSI.Application.Services.Master
{


    [ApiController, Route("service/mscluster")]
    public class MSClusterServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;

        public MSClusterServiceController(HttpRequestHelper htr)
        {
            _htr = htr;
        }

        [Authorize, HttpPost, Route("list")]
        public async Task<IActionResult> GetList()
        {
            //ResponseModel result = await _htr.AsyncAsResponseModel(JsonSerializer.Serialize(param), "ApiClient", "/api/dailyactivity/getdetailformula");
            var result = await _htr.PostAsyncDataTable<List<MSClusterDto>>(
                "{}",
                "/api/mscluster/list"
            );
            return this.ResponseStatus(result);
           
            //return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("detail")]
        public async Task<IActionResult> GetData([FromBody] MSCluster payload)
        {
            var result = await _htr.PostAsyncDataTable<MSClusterDto>(
                JsonSerializer.Serialize(payload),
                "/api/mscluster/detail"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });

        }

        [Authorize, HttpPost, Route("create")]
        public async Task<IActionResult> Create([FromBody] MSCluster payload)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(payload),
                "/api/mscluster/create",
                HttpMethod.Post
            );


            //if (result.Content != null)
            //{
            //    var contentStr = result.Content.ToString();

            //    try
            //    {
            //        var obj = JsonSerializer.Deserialize<ResponseModel>(contentStr);

            //        if (obj != null)
            //            message = obj.Message;
            //    }
            //    catch
            //    {
            //        message = contentStr;
            //    }
            //}
            return this.ResponseStatus(result);
            //return Json(new { success = result.Status, message = result.Message });

        }

        [Authorize, HttpPatch, Route("update")]
        public async Task<IActionResult> Update([FromBody] MSCluster payload)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(payload),
                "/api/mscluster/update",
                HttpMethod.Patch
            );
             return this.ResponseStatus(result);
            //return Json(new { success = result.Status, message = result.Message });
        }


        //delete
        [Authorize, HttpPost, Route("delete")]
        public async Task<IActionResult> Delete([FromBody] MSCluster payload)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(payload),
                "/api/mscluster/remove",
                HttpMethod.Post
            );
            return this.ResponseStatus(result);
            //return Json(new { success = result.Status, message = result.Message });
        }
    }
}

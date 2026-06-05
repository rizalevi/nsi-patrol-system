using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Extensions;
using NSI.Domain.DTOs.Dashboard;
using NSI.Domain.DTOs.Tasklist;
using NSI.Infra.Helpers;
using System.Security.Claims;

namespace NSI.Application.Services.Tasklist
{


    [ApiController, Route("service/tasklist")]
    public class TasklistServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;

        public TasklistServiceController(HttpRequestHelper htr)  
        {
            _htr = htr;
        }

        [Authorize, HttpPost, Route("get-usergroup")]
        public async Task<IActionResult> GetUserGroup()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncStringList<List<TaskGroupUserDto>>(
                userId,
                "api/tasklist/getusergroup",
                HttpMethod.Post

            );
            return this.ResponseStatus(result);
            //return Json(new {
            //    status = result.Status,
            //    message = result.Message,
            //    data = result.Data });
        }

        [Authorize, HttpPost, Route("get-summary")]
        public async Task<IActionResult> GetCourse()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncStringList<List<SummaryProcessDto>>(
               userId,
               "api/tasklist/summary",
               HttpMethod.Post
            );

            return this.ResponseStatus(result);
            //return Json(new {
            //    status = result.Status,
            //    message = result.Message,
            //    data = result.Data });
        }


        [Authorize, HttpPost, Route("list")]
        public async Task<IActionResult> GetList()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncStringList<List<TaskListDto>>(
                userId,
                "api/tasklist/list",
                HttpMethod.Post
            );
            return this.ResponseStatus(result);
            //return Json(new {
            //    status = result.Status,
            //    message = result.Message,
            //    data = result.Data });
        }



    }
}

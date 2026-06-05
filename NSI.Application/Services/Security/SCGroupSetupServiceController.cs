using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Extensions;
using NSI.Domain.DTOs.Security;
using NSI.Infra.Helpers;
using System.Security.Claims;
using System.Text.Json;

namespace NSI.Application.Services.Security
{

    [ApiController, Route("service/groupsetup")]
    public class SCGroupSetupServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;

        public SCGroupSetupServiceController(HttpRequestHelper htr)
        {
            _htr = htr;
        }

        [Authorize, HttpPost, Route("group-ddl")]
        public async Task<IActionResult> GetGroupDDL()
        {
            var result = await _htr.PostAsyncDataTable<List<GroupSetupDDLDto>>(
                "{}",
                "api/groupsetup/group-ddl"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("list")]
        public async Task<IActionResult> GetList()
        {

            var result = await _htr.PostAsyncDataTable<List<GroupSetupDto>>(
                "{}",
                "api/groupsetup/list"
            ); 
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("detail")]
        public async Task<IActionResult> GetData([FromBody] GroupSetup payload)
        {
            var result = await _htr.PostAsyncDataTable<GroupSetup>(
                JsonSerializer.Serialize(payload),
                "api/groupsetup/detail"
            );

            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });

        }

        [Authorize, HttpPost, Route("save")]
        public async Task<IActionResult> Create([FromBody] GroupSetup payload)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(payload),
                "api/groupsetup/save",
                HttpMethod.Post
            );

            return this.ResponseStatus(result);

        }



        //delete
        [Authorize, HttpPost, Route("delete")]
        public async Task<IActionResult> Delete([FromBody] GroupSetup payload)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(payload),
                "api/groupsetup/remove",
                HttpMethod.Post
            );
            return this.ResponseStatus(result);
        }


        //privilege
        [Authorize, HttpPost, Route("privilege")]
        public async Task<IActionResult> GetPrivilege([FromBody] GroupSetup model)
        {
            var result = await _htr.PostAsyncDataTable<List<GroupPrivilegeDto>>(
                JsonSerializer.Serialize(model),
                "api/groupsetup/privilege"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }

        //coursesetting
        [Authorize, HttpPost, Route("coursesetting")]
        public async Task<IActionResult> GetCourseSetting([FromBody] GroupSetup model)
        {
            var result = await _htr.PostAsyncDataTable<List<CourseSettingDto>>(
                JsonSerializer.Serialize(model),
                "api/groupsetup/course-setting"
            );
            return this.ResponseStatus(result);

            //return Json(new { data = result.Data });
        }
    }
}

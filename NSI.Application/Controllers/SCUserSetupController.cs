using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Filters;
using NSI.Domain.Models.Auth;
using NSI.Infra.Helpers;

namespace NSI.Application.Controllers
{
    [Route("SCUserSetup")]
    public class SCUserSetupController : BaseController
    {

        public SCUserSetupController(HttpRequestHelper htr) : base(htr) { }


        [Authorize]
        [AuthorizeMenu]
        [HttpGet("index")]
        public async Task<IActionResult> Index(string ID)
        {
            var permission = await GetPrivilegeAsync(ID);
           
            ViewBag.Permission = permission;

            return View();
        }


        [HttpGet("profile")]
        public async Task<IActionResult> Profile(string ID)
        {

            var permission = await GetPrivilegeAsync(ID);

            return View("Profile");
        }


    }
}

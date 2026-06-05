using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Filters;
using NSI.Infra.Helpers;

namespace NSI.Application.Controllers
{
    [Route("MSDevice")]
    public class MSDeviceController : BaseController
    {
        public MSDeviceController(HttpRequestHelper htr) : base(htr) { }

        [Authorize]
        [AuthorizeMenu]
        [HttpGet("index")]
        public async Task<IActionResult> Index(string ID)
        {
            var permission = await GetPrivilegeAsync(ID);
           
                
            ViewBag.Permission = permission;

            return View();
        }
    }
}

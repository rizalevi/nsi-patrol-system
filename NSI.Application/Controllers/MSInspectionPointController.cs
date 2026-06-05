using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Filters;
using NSI.Infra.Helpers;

namespace NSI.Application.Controllers
{
    [Route("MSInspectionPoint")]
    public class MSInspectionPointController : BaseController
    {
        public MSInspectionPointController(HttpRequestHelper htr) : base(htr) { }

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

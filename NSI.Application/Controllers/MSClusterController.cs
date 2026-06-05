using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Filters;
using NSI.Infra.Helpers;

namespace NSI.Application.Controllers
{
    [Route("MSCluster")]
    public class MSClusterController : BaseController
    {
        public MSClusterController(HttpRequestHelper htr) : base(htr) { }

        [Authorize]
        [AuthorizeMenu]
        [HttpGet("Index")]
        public async Task<IActionResult>Index(string ID)
        {
            var permission = await GetPrivilegeAsync(ID);
            

            ViewBag.Permission = permission;
            //ViewData["AllowDelete"] = "1";
            return View("Index");
        }

    }
}

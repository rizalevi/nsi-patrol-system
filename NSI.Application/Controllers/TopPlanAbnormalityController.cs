using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Filters;
using NSI.Infra.Helpers;

namespace NSI.Application.Controllers
{
    [Route("TopPlanAbnormality")]
    public class TopPlanAbnormalityController : BaseController
    {
        public TopPlanAbnormalityController(HttpRequestHelper htr) : base(htr) { }


        [Authorize]
        [AuthorizeMenu]
        [HttpGet("Plant")]
        public async Task<IActionResult> Plant(string ID)
        {
            var permission = await GetPrivilegeAsync(ID);

            ViewBag.Permission = permission;
            return View("Plant");

        }

        [Authorize]
        [AuthorizeMenu]
        [HttpGet("Course")]
        public async Task<IActionResult> Course(string ID)
        {
            var permission = await GetPrivilegeAsync(ID);

            ViewBag.Permission = permission;
            return View("Course");

        }

        [Authorize]
        [AuthorizeMenu]
        [HttpGet("Inspection")]
        public async Task<IActionResult> Inspection(string ID)
        {
            var permission = await GetPrivilegeAsync(ID);

            ViewBag.Permission = permission;
            return View("Inspection");

        }
    }
}

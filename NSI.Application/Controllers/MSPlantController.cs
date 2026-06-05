using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Filters;
using NSI.Infra.Helpers;


namespace NSI.Application.Controllers
{
    [Route("MSPlant")]
    public class MSPlantController : BaseController
    {
        public MSPlantController(HttpRequestHelper htr) : base(htr) { }


        [Authorize]
        [AuthorizeMenu]
        [HttpGet("index")]
        public async Task<IActionResult>Index(string ID)
        {
            var permission = await GetPrivilegeAsync(ID);
          

            ViewBag.Permission = permission;

            return View();
        }

    }
}

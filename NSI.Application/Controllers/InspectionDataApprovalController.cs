using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Filters;
using NSI.Infra.Helpers;


namespace NSI.Application.Controllers
{
    [Route("InspectionDataApproval")]
    public class InspectionDataApprovalController : BaseController
    {
        public InspectionDataApprovalController(HttpRequestHelper htr) : base(htr) { }

        [Authorize]
        [AuthorizeMenu]
        [HttpGet("index")]
        public async Task<IActionResult> Index(string ID)
        {
            var permission = await GetPrivilegeAsync(ID);
           

            ViewBag.Permission = permission;

            return View();
        }

        [HttpPost("Detail")]
        public async Task<IActionResult> Detail(string ID, string ClusterCode, string PlantCode, string CourseCode, string GroupCode, string InspectionDate, string Frequency)
        {
            var permission = await GetPrivilegeAsync(ID);
            ViewBag.Permission = permission;

            ViewData["ClusterCode"] = ClusterCode;
            ViewData["PlantCode"] = PlantCode;
            ViewData["CourseCode"] = CourseCode;
            ViewData["GroupCode"] = GroupCode;
            ViewData["InspectionDate"] = InspectionDate;
            ViewData["Frequency"] = Frequency;
            return View("Index");
        }


    }
}

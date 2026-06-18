using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Filters;
using NSI.Domain.DTOs.Email;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.Interfaces;
using NSI.Infra.Helpers;
using System.Security.Claims;
using System.Text.Json;


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

        //[HttpPost("Detail")]
        //public async Task<IActionResult> Detail(string ID, string ClusterCode, string PlantCode, string CourseCode, string GroupCode, string InspectionDate, string Frequency)
        //{
        //    var permission = await GetPrivilegeAsync(ID);
        //    ViewBag.Permission = permission;

        //    ViewData["ClusterCode"] = ClusterCode;
        //    ViewData["PlantCode"] = PlantCode;
        //    ViewData["CourseCode"] = CourseCode;
        //    ViewData["GroupCode"] = GroupCode;
        //    ViewData["InspectionDate"] = InspectionDate;
        //    ViewData["Frequency"] = Frequency;
        //    return View("Index");
        //}

        [Authorize]
        [HttpGet("Email")]
        public async Task<IActionResult> Email(Guid token)
        {
            var link =await GetDataFromLink(token,"0");

            if (link == null)
            {
                return RedirectToAction("Index","Home");
            }

            if (!link.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            var payload =  JsonSerializer.Deserialize< List<InspectionPayloadEmail>>(link.Payload);

            var p = payload?.FirstOrDefault();

            if (p == null)
            {
                return RedirectToAction("Index", "Home");
            }


            var permission = await GetPrivilegeFromEmailAsync(p.ID, link.UserID);

            ViewBag.Permission = permission;
            ViewData["ClusterCode"] =p.ClusterCode;
            ViewData["PlantCode"] =p.PlantCode;
            ViewData["CourseCode"] = p.CourseCode;
            ViewData["GroupCode"] =p.GroupCode;
            ViewData["InspectionDate"] = p.InspectionDate;
            ViewData["Frequency"] = p.Frequency;
         
            return View("Index");
        }

        [Authorize]
        [HttpGet("Task")]
        public async Task<IActionResult> Task(Guid ID)
        {
            var link = await GetDataFromLink(ID, "1");

            //if (link == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            var payload = JsonSerializer.Deserialize<List<InspectionPayloadEmail>>(link.Payload);

            var p = payload?.FirstOrDefault();

            if (p == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var permission = await GetPrivilegeAsync("B03");

            ViewBag.Permission = permission;
            ViewData["ClusterCode"] = p.ClusterCode;
            ViewData["PlantCode"] = p.PlantCode;
            ViewData["CourseCode"] = p.CourseCode;
            ViewData["GroupCode"] = p.GroupCode;
            ViewData["InspectionDate"] = p.InspectionDate;
            ViewData["Frequency"] = p.Frequency;

            return View("Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace NSI.Application.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet("NoAccess")]
        public IActionResult NoAccess(string menuId, string menuName)
        {
            ViewBag.MenuId = menuId;
            ViewBag.MenuName = menuName;

            return View();
        }

        [HttpGet("NotFound")]
        public IActionResult NotFoundPage()
        {
            return View();
        }

        [HttpGet("ServerError")]
        public IActionResult ServerError()
        {
            return View();
        }


        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

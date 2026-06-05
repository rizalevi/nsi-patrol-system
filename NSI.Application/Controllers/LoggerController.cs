using Microsoft.AspNetCore.Mvc;
using NSI.Infra.Helpers;

namespace NSI.Application.Controllers
{
    public class LoggerController : BaseController
    {
        public LoggerController(HttpRequestHelper htr) : base(htr) { }

     
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NSI.Application.Controllers
{
    public class TasklistRemainingController : Controller
    {

        [Authorize]
       
        public async Task<IActionResult>Index()
        {
            ViewData["AllowDelete"] = "1";
            return View();
           
        }


    }
}

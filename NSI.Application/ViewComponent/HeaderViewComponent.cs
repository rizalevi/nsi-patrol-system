//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using ADLESIPRO_Master.Models;
//using Microsoft.AspNetCore.DataProtection;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;

//namespace ADLESIPRO_Master.ViewComponents
//{
//    public class HeaderViewComponent : ViewComponent
//    {
//        private readonly IConfiguration _configuration;
//        private readonly IWebHostEnvironment _webHostEnvironment;
//        private readonly IDataProtector _dataProtector;
//        private string connectionString;
//        public HeaderViewComponent(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IDataProtectionProvider dataProtectionProvider)
//        {
//            _configuration = configuration;
//            _webHostEnvironment = webHostEnvironment;
//            _dataProtector = dataProtectionProvider.CreateProtector("UserLoginCookieProtection");
//        }

//        private string DecryptCookie(string encryptedValue)
//        {
//            try
//            {
//                return _dataProtector.Unprotect(encryptedValue);
//            }
//            catch (System.Exception)
//            {
//                return string.Empty;
//            }
//        }

//        public Task<IViewComponentResult> InvokeAsync()
//        {
//            //==================================================================
//            // Viewbag for showing UserID, FullName and GroupID
//            //==================================================================
//            ViewBag.UserID = DecryptCookie(Request.Cookies["UserID"]);
//            ViewBag.FullName = DecryptCookie(Request.Cookies["FullName"]);
//            ViewBag.UserGroupID = DecryptCookie(Request.Cookies["UserGroupID"]);

//            List<Menu> yourModelClass = new List<Menu>();
//            return Task.FromResult<IViewComponentResult>(View(yourModelClass));
//        }
//    }
//}

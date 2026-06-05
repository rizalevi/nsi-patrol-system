using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.DTOs;
using NSI.Infra.Helpers;
using System.Net;
using System.Text.Json;


namespace NSI.Application.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        //private readonly IConfiguration _configuration;
        //private readonly IWebHostEnvironment _webHostEnvironment;
        //private readonly IDataProtector _dataProtector;
        //private string connectionString;

        private readonly HttpRequestHelper _htr;

        public SidebarViewComponent(HttpRequestHelper htr)
        {
            _htr = htr;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return View(new List<UserMenuDto>());

            var response = await _htr.AsyncAsResponseModel<object>(
                "{}",
                "/api/menu/list",
                HttpMethod.Post);

            if (response.StatusCode == HttpStatusCode.Unauthorized ||
             response.StatusCode == HttpStatusCode.Forbidden)
            {
                HttpContext.Response.Redirect("/Account/Login");
                return Content(string.Empty);
            }

            if (!response.Status || response.Data == null)
                return View(new List<UserMenuDto>());

            var json = response.Data is string s
                ? s
                : JsonSerializer.Serialize(response.Data);

            var menus = JsonSerializer.Deserialize<List<UserMenuDto>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<UserMenuDto>();

            return View(menus);

           // if (!User.Identity.IsAuthenticated)
           //     return View(new List<UserMenuDto>());

           // var response = await _htr.AsyncAsResponseModel(
           //     "{}", // body kosong
           //     "/api/menu/list",
           //     HttpMethod.Post
           // );

           // if (!response.Status || string.IsNullOrWhiteSpace(response.Content?.ToString()))
           //     return View(new List<UserMenuDto>());
           // var jsonSerialize = JsonSerializer.Deserialize<List<UserMenuDto>>(
           //    response.Content.ToString()!,
           //    new JsonSerializerOptions
           //    {
           //        PropertyNameCaseInsensitive = true
           //    }
           //) ?? new List<UserMenuDto>();

           //// List<UserMenuDto> GroupMenuList = new List<UserMenuDto>();
           // //return Task.FromResult<IViewComponentResult>(View(GroupMenuList));
           // return View(jsonSerialize);
        }

        //public SidebarViewComponent(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IDataProtectionProvider dataProtectionProvider)
        //{
        //    _configuration = configuration;
        //    _webHostEnvironment = webHostEnvironment;
        //    _dataProtector = dataProtectionProvider.CreateProtector("UserLoginCookieProtection");
        //}

        //private string DecryptCookie(string encryptedValue)
        //{
        //    try
        //    {
        //        return _dataProtector.Unprotect(encryptedValue);
        //    }
        //    catch (System.Exception)
        //    {
        //        return string.Empty;
        //    }
        //}

//        public Formatting serializerSettings { get; private set; }

//        public Task<IViewComponentResult> InvokeAsync()
//        {
//            /*==================================================================
//            Set Mode Developtment / Production
//            ==================================================================*/
//#if DEBUG
//            //Mode Debug/Developtment
//            ViewBag.IsDebugMode = true;
//#else
//                        //Mode Release/Production
//                        ViewBag.IsDebugMode = false;
//#endif
//            /*==================================================================*/

//            List<GroupMenu> GroupMenuList = new List<GroupMenu>();
//            connectionString = _configuration.GetConnectionString("ConnectionString");

//            //==================================================================
//            // Viewbag for showing UserLogin & UserName
//            //==================================================================
//            ViewBag.UserID = DecryptCookie(Request.Cookies["UserID"]);
//            ViewBag.FullName = DecryptCookie(Request.Cookies["FullName"]);
//            ViewBag.UserGroupID = DecryptCookie(Request.Cookies["UserGroupID"]);

//            //==================================================================
//            // Get list Group Menu 
//            //==================================================================
//            var UserGroupID = DecryptCookie(Request.Cookies["UserGroupID"]);
//            MenuPrm prm = new MenuPrm();
//            prm.UserGroupID = UserGroupID;

//            var apiURL = DecryptCookie(Request.Cookies["apiURL"]) + DecryptCookie(Request.Cookies["APIPort_Main"]);
//            var client = new HttpClient();

//            HttpResponseMessage response = client.PostAsJsonAsync(apiURL + "api/Home/Menu", prm).Result;

//            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest)
//            {
//                string apiResponse = response.Content.ReadAsStringAsync().Result;
//                Response resp = JsonConvert.DeserializeObject<Response>(apiResponse);
//                GroupMenuList = JsonConvert.DeserializeObject<List<GroupMenu>>(resp.Contents.ToString());
//            }
//            return Task.FromResult<IViewComponentResult>(View(GroupMenuList));
//        }
    }
}

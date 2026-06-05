
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Infra.Helpers;
using System.Text.Json;
using NSI.Domain.DTOs;

public class HomeController : Controller
{
    private readonly HttpRequestHelper _htr;
    private readonly bool isHttpSecure = false;
    private readonly IConfiguration _config;

    public HomeController(HttpRequestHelper htr, IConfiguration config) // , IRefreshTokenService refreshTokenService
    {
        //  _authApi = authApi;
        _htr = htr;
        _config = config;
        //_refreshTokenService = refreshTokenService;
        isHttpSecure = Convert.ToBoolean(config["AppSettings:IsHttpSecure"]);

    }

    [Authorize]
    public async Task<IActionResult> Index()
    {

        var menus = await GetSideMenusAsync();


        ViewBag.Menus = menus;
        return View();
    }

    //[Authorize]
    //public async Task<IActionResult> SideMenu()
    //{
    //    UserMenu userMenu = new UserMenu();
    //    var menus = await _htr.AsyncAsResponseModel(
    //                JsonSerializer.Serialize(userMenu),
    //                "/api/menu/list"
    //            );
    //    //var menus = await _htr.GetAsync<List<UserMenu>>(
    //    //    $"/api/menu/{UserId}"
    //    //);

    //    //ViewBag.Menus = menus;
    //    return View(menus);
    //}

    private async Task<List<UserMenuDto>> GetSideMenusAsync()
    {

        var response = await _htr.AsyncAsResponseModel<object>(
            "{}", // body kosong
            "/api/menu/list",
            HttpMethod.Post
        );


        if (!response.Status)
        {
            //redirect to login
            return new List<UserMenuDto>();


        }
        
        //if (!response.Status || response.Content == null)   
        //    return new List<UserMenu>();

        //return JsonSerializer.Deserialize<List<UserMenu>>(
        //    response.Content.ToString()!
        //) ?? new List<UserMenu>();
        if (!response.Status || string.IsNullOrWhiteSpace(response.Data?.ToString()))
            return new List<UserMenuDto>();

        return JsonSerializer.Deserialize<List<UserMenuDto>>(
            response.Data.ToString()!,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        ) ?? new List<UserMenuDto>();

      
        
    }


}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.DTOs;
using NSI.Infra.Helpers;
using System.Net;
using System.Text.Json;

public class MenuViewComponent : ViewComponent
{
    private readonly HttpRequestHelper _htr;

    public MenuViewComponent(HttpRequestHelper htr)
    {
        _htr = htr;
    }

    [Authorize]
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
    }

    //public async Task<IViewComponentResult> InvokeAsync()
    //{
    //    if (!User.Identity.IsAuthenticated)
    //        return View(new List<UserMenuDto>());

    //    var response = await _htr.AsyncAsResponseModel(
    //        "{}", // body kosong
    //        "/api/menu/list",
    //        HttpMethod.Post
    //    );

    //    if (!response.Status || string.IsNullOrWhiteSpace(response.Content?.ToString()))
    //        return View(new List<UserMenuDto>());
    //    var jsonSerialize= JsonSerializer.Deserialize<List<UserMenuDto>>(
    //       response.Content.ToString()!,
    //       new JsonSerializerOptions
    //       {
    //           PropertyNameCaseInsensitive = true
    //       }
    //   ) ?? new List<UserMenuDto>();
    //    return View(jsonSerialize);
    //}
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NSI.Infra.Helpers;
using NSI.Domain.DTOs.Security;
using System.Security.Claims;
using System.Text.Json;
using NSI.Domain.Models.Logger;
using NSI.Infra.Repositories.Logger;
using NSI.Domain.Interfaces.Logger;

public class AuthorizeMenuFilter : IAsyncAuthorizationFilter
{
    private readonly HttpRequestHelper _htr;
    private IErrorLogRepository _repo;

    public AuthorizeMenuFilter(HttpRequestHelper htr, IErrorLogRepository repo)
    {
        _htr = htr;
        _repo = repo;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        // kalau belum login → biarin Authorize handle
        if (!user.Identity.IsAuthenticated)
            return;

        // ambil MenuID dari query (?ID=...)
        var menuId = context.HttpContext.Request.Query["ID"].ToString();

        if (string.IsNullOrEmpty(menuId))
        {
            context.Result = new RedirectToActionResult("NoAccess", "Error", null);
            return;
        }

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        var param = new PermissionParamDto
        {
            MenuID = menuId,
            UserID = userId
        };

        var response = await _htr.AsyncAsResponseModel<PermissionDto>(
            JsonSerializer.Serialize(param),
            "/api/userpermission/get-privilege",
            HttpMethod.Post
        );

        var permission = response.Data;

        //if (permission?.AllowAccess != "1")
        //{
        //    context.Result = new RedirectToActionResult("NoAccess", "Error", null);
        //    return;
        //}

        if (permission?.AllowAccess != "1")
        {
            //var logger = context.HttpContext.RequestServices.GetRequiredService<ErrorLogRepository>();

            await _repo.LogAsync(new ErrorLogModel
            {
                Message = "Unauthorized access",
                Path = context.HttpContext.Request.Path,
                Method = context.HttpContext.Request.Method,
                UserId = context.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                QueryString = context.HttpContext.Request.QueryString.ToString(),
                RequestId = context.HttpContext.TraceIdentifier,
                StackTrace = "403 - Access Denied\r\nYou do not have permission to access this menu.\r\nPlease contact administrator if you need access."
            });
            

            context.Result = new RedirectToActionResult(
                "NoAccess",
                "Error",
                new
                {
                    menuId = menuId,
                    menuName = permission?.MenuName ?? "Unknown Menu"
                }
            );
            return;
        }


        // lempar ke controller biar bisa dipakai ViewBag
        context.HttpContext.Items["Permission"] = permission;
    }
}
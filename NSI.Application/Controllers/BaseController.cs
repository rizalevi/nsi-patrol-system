using Microsoft.AspNetCore.Mvc;
using NSI.Application.Services;
using NSI.Domain.DTOs.Security;
using NSI.Infra.Helpers;
using System.Security.Claims;
using System.Text.Json;


namespace NSI.Application.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly HttpRequestHelper _htr;

        public BaseController(HttpRequestHelper http)
        {
            _htr = http;
        }

        protected string UserId =>
            HttpContext.Session.GetString("UserID") ?? "";

        protected async Task<PermissionDto?> GetPrivilegeAsync(string menuId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = new PermissionParamDto
            {
                MenuID = menuId,
                UserID = userId
            };
            
            var response = await _htr.AsyncAsResponseModel<PermissionDto>(
                JsonSerializer.Serialize(model),
                "/api/userpermission/get-privilege",
                HttpMethod.Post
            );

            return response.Data;

        }
    }
}

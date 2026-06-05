using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.Interfaces;
using NSI.Domain.Models.Auth;
using System.Security.Claims;


[ApiController]
[Route("api/menu")]
public class MenuController : ControllerBase
{
    private readonly IMenuRepository _repo;

    public MenuController(IMenuRepository repo)
    {
        _repo = repo;
    }


    /// <summary>
    /// Get side menu by UserID
    /// </summary>
    /// 
    [Authorize]
    [HttpPost("list")]
    public async Task<ApiResponse<object>> SideMenu()
    {

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            // Kalau userId null → redirect ke login
            return new ApiResponse<object> { Status = false, Message = MessageModel.InvalidToken, Data = null };

        }

        var menus = await _repo.GetMenusAsync(userId);
        return new ApiResponse<object> { Status = true, Message = "ok", Data = menus };

    }


}

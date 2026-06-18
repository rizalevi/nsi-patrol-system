using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Api.Auth;
using NSI.Domain.DTOs;
using NSI.Domain.DTOs.Email;
using NSI.Domain.Interfaces;
using NSI.Domain.Models.Auth;
using NSI.Domain.Models.Master;

//using NSI.Domain.Services;
using NSI.Infra.Helpers;
using NSI.Infra.Services;
using System.Collections.Concurrent;

namespace NSI.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
  
    private readonly JwtService _jwtService;
    //private readonly RefreshTokenService _refreshTokenService;
    //private readonly IRefreshTokenService _refreshTokenService;

    private readonly RateLimitService _rateLimitService;

    private static readonly ConcurrentDictionary<string, (int Count, DateTime WindowStart)>
            _loginAttempts = new();
    private readonly IAuthRepository _authRepository;

    private readonly int MaxLoginAttempts;
    private readonly int PenaltyTimeInMinutes=5;

    public AuthController(JwtService jwtService,   RateLimitService rateLimitService, IAuthRepository authRepository) //IRefreshTokenService refreshTokenService, 
    {
        _jwtService = jwtService;
        //_refreshTokenService = refreshTokenService; //matikan dulu
        _rateLimitService = rateLimitService;
        _authRepository = authRepository;
    }

    [HttpPost("login")]
    public async Task<ApiResponse<object>> Login([FromBody] Login request)
    {
        //string? clientIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
        string? clientIp = request.IPAddress ?? "";  //HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";

        string userAgent = HttpContext?.Request?.Headers?.UserAgent.ToString() ?? "";

        var userId = request.UserId?.Trim().ToUpper() ?? "";
        var now = DateTime.UtcNow;


        try
        {
            await _rateLimitService.CheckAsync(clientIp, userId);
        }
        catch (UnauthorizedAccessException ex)
        {
            //return StatusCode(429, new { code = 429, message = ex.Message });
            return new ApiResponse<object> { Status = false, Message = $"Too much failed login attempts. Try again after {PenaltyTimeInMinutes} minute(s)", Data = null };
        }


        AuthModel dbUserData = new();

        if (string.IsNullOrEmpty(request.UserId))
        {
            return new ApiResponse<object> { Status = false, Message = "Please fill UserID", Data = null };
        }

        if (string.IsNullOrEmpty(request.Password))
        {
            return new ApiResponse<object> { Status = false, Message = "Please fill Password", Data = null };
        }


        //cek user id
        var user = await _authRepository.GetByUserIdAsync(userId);
        var userName="";
        var email = "";
        var avatar = "";

        if (user.UserId  == null)
        {
            await _rateLimitService.RegisterFailureAsync(clientIp, userId);
            return new ApiResponse<object> { Status = false, Message = "Invalid User ID or Password", Data = null };
        }
        else
        {
            userName = user.UserName;
            avatar = user.PicturePath;
            email = user.Email;
        }

        // ambil password terenkripsi dari DB
        var encryptedPassword =
                await _authRepository.GetEncryptedPasswordAsync(userId);

        if (encryptedPassword == null)
        {
            await _rateLimitService.RegisterFailureAsync(clientIp, userId);
            return new ApiResponse<object> { Status = false, Message = "Invalid User ID or Password", Data = null };
        }

        // decrypt password
        string decryptedPassword;
        try
        {
            decryptedPassword =
                CryptHelper.PasswordDecrypt(encryptedPassword, userId);
        }
        catch
        {
            await _rateLimitService.RegisterFailureAsync(clientIp, userId);
            return new ApiResponse<object> { Status = false, Message = "Invalid User ID or Password", Data = null };
        }

        // compare password
        if (decryptedPassword != request.Password)
        {
            await _rateLimitService.RegisterFailureAsync(clientIp, userId);
            return new ApiResponse<object> { Status = false, Message = "Invalid User ID or Password", Data = null };
        }

        // check exp password

        var expPass =  await _authRepository.CheckExpiredPassword(userId);
        if (expPass <= DateTime.Now.Date)
        {
            return new ApiResponse<object>
            {
                Status = false,
                Message = "Password expired! Please contact administrator",
                Data = null
            };
        }

        await _rateLimitService.ResetAsync(clientIp, userId);


        //var token = _jwtService.GenerateToken(request.UserId);
        var jwtToken = _jwtService.GenerateToken(request.UserId, out DateTime expiredAt); // kita ubah GenerateToken untuk mengembalikan expiredAt juga
        //var refresh = await _refreshTokenService.CreateAsync(request.UserId);

        dbUserData.AccessToken = jwtToken;
       // dbUserData.RefreshToken = refresh;
        dbUserData.ExpiredAt = expiredAt;
        dbUserData.UserName = userName + "";
        dbUserData.PicturePath = avatar + "";
        dbUserData.Email = email;

        return new ApiResponse<object> { Status = true, Message = "ok", Data = dbUserData };

        //return Ok(new LoginResponse
        //{
        //    Token = jwtToken,
        //    ExpiredAt= expiredAt,
           
        //});
    }

    [AllowAnonymous]
    [HttpPost("email-login")]
    public async Task<ApiResponse<object>> EmailLogin(
    [FromBody] EmailLoginDto request)
    {
        var link = await _authRepository.GetByTokenAsync(request.Token,"0");

        if (link == null)
        {
            return new ApiResponse<object>
            {
                Status = false,
                Message = "Invalid token"
            };
        }

        if (!link.IsValid)
        {
            return new ApiResponse<object>
            {
                Status = false,
                Message = "Link expired"
            };
        }

        var user = await _authRepository.GetByUserIdAsync(link.UserID);

        var jwtToken =
            _jwtService.GenerateToken(
                link.UserID,
                out DateTime expiredAt);

        await _authRepository.MarkEmailLinkVisitedAsync(request.Token);

        return new ApiResponse<object>
        {
            Status = true,
            Message = "OK",
            Data = new AuthModel
            {
                UserId = link.UserID,
                UserName = user.UserName,
                Email = user.Email,
                PicturePath = user.PicturePath,
                AccessToken = jwtToken,
                ExpiredAt = expiredAt,
                //link url from email
                RedirectAction = link.RedirectAction,
                Payload = link.Payload
            }
        };
    }


    // =====================
    // CHANGE PASSWORD --matiin dlu 
    // =====================
    //[Authorize]
    //[HttpPost("change-password")]
    //public async Task<IActionResult> ChangePassword(
    // [FromBody] ChangePasswordRequest request)
    //{
    //    if (!ModelState.IsValid)
    //        return BadRequest(ModelState);

    //    var userId = User.Identity!.Name!;

    //    var success = await _authRepository.ChangePasswordAsync(
    //        userId,
    //        request.OldPassword,
    //        request.NewPassword);

    //    if (!success)
    //        return BadRequest(new { message = "Old password is incorrect" });

    //    // 🔥 AUTO REVOKE SEMUA REFRESH TOKEN
    //    await _refreshTokenService.RotateAsync(token) /*RevokeAsync(userId);*/

    //    Response.Cookies.Delete("refresh_token");

    //    return Ok(new
    //    {
    //        message = "Password changed successfully. Please login again."
    //    });
    //}


    //[AllowAnonymous]
    //[HttpPost("refresh")]
    //public async Task<IActionResult> Refresh()
    //{
    //    if (!Request.Cookies.TryGetValue("refresh_token", out var rawToken))
    //        return Unauthorized();

    //    var stored = await _refreshTokenService.ValidateAsync(rawToken);
    //    if (stored == null)
    //        return Unauthorized();

    //    await _refreshTokenService.RevokeAsync(rawToken); //(stored);

    //    var newJwt = _jwtService.GenerateToken(
    //        stored.UserId,
    //        out DateTime expiredAt
    //    );

    //    var newRefresh = await _refreshTokenService.CreateAsync(stored.UserId);

    //    //Response.Cookies.Append("refresh_token", newRefresh, new CookieOptions
    //    //{
    //    //    HttpOnly = true,
    //    //    Secure = true,
    //    //    SameSite = SameSiteMode.Strict,
    //    //    Expires = DateTime.UtcNow.AddDays(7)
    //    //});

    //    return Ok(new { token = newJwt, expiredAt });
    //}



    //// =====================
    //// LOGOUT
    //// =====================
    [HttpPost("logout")]
    public async Task<ApiResponse<object>> Logout()
    {
    //    if (Request.Cookies.TryGetValue("refresh_token", out var rawToken))
    //    {
    //        var stored = await _refreshTokenService.ValidateAsync(rawToken);
    //        if (stored != null)
    //        {
    //            await _refreshTokenService.RotateAsync(stored);
    //        }
    //    }

        //Response.Cookies.Delete("refresh_token");

        AuthModel mdl = new AuthModel
        {
            AccessToken = null,
            ExpiredAt = DateTime.MinValue

        };
        return new ApiResponse<object> { Status = true, Message = "Logged out", Data = mdl };


        //return Ok(new { message = "Logged out" });
    }


    //buat email link redirect menu transaction

    //[HttpPost("get-token-email")]
    //public async Task<ApiResponse<object>> GetByTokenAsync([FromBody] Guid token)
    //{
    //    var result = await _authRepository.GetByTokenAsync(token);
    //    return new ApiResponse<object>
    //    {
    //        Status = true,
    //        Message = "OK",
    //        Data = new AuthModel
    //        {
    //            Payload = result.Payload
    //        }
    //    };


    //}

    [HttpPost("redirect-key")]
    public async Task<ApiResponse<EmailLoginInfoDto>>GetByTokenAsync([FromBody] RedirectKeyRequest request)
    {
        var result =
            await _authRepository.GetByTokenAsync(request.Token, request.Type);

        return new ApiResponse<EmailLoginInfoDto>
        {
            Status = true,
            Message = "OK",
            Data = result
        };
    }

    [AllowAnonymous]
    [HttpGet("ipaddress")]
    public IActionResult MyIp()
    {
        return Ok(new
        {

            RemoteIp =
                HttpContext.Connection.RemoteIpAddress?.ToString(),

            XForwardedFor =
                Request.Headers["X-Forwarded-For"].ToString(),

            XRealIp =
                Request.Headers["X-Real-IP"].ToString()
        });
    }

}


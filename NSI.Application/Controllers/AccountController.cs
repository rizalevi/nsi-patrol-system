using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.Models.Auth;
using System.Security.Claims;
using NSI.Infra.Helpers;
using System.Text.Json;
//using NSI.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using NSI.Domain.Models.Security;

namespace NSI.Application.Controllers
{
    public class AccountController : Controller
    {
        //private readonly IAuthApiService _authApi;
        private readonly HttpRequestHelper _htr;
        private readonly bool isHttpSecure = false;
        private readonly IConfiguration _config;
        //private readonly IRefreshTokenService _refreshTokenService;


        public AccountController(HttpRequestHelper htr, IConfiguration config)  //, IRefreshTokenService refreshTokenService
        {
            //  _authApi = authApi;
            _htr = htr;
            _config = config;
            //_refreshTokenService = refreshTokenService;
            isHttpSecure = Convert.ToBoolean(config["AppSettings:IsHttpSecure"]);
           // _refreshTokenService = refreshTokenService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {

            HttpMethod method = HttpMethod.Post;

            var result = new ApiResponse<object>();
            try
            {
                result = await _htr.AsyncAsResponseModel<object>(
                    JsonSerializer.Serialize(model),
                    "/api/auth/login",
                    method

                );

                if (!result.Status)
                {
                    return BadRequest(result);
                }

                var jsonElement = (JsonElement)result.Data;
                var rawJson = jsonElement.GetRawText();

                var data = JsonSerializer.Deserialize<AuthModel>(
                    rawJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

                //var tokenHandler = new JwtSecurityTokenHandler();

                //data.AccessToken = CryptHelper.AesDecrypt(data.AccessToken);
                //data.RefreshToken = CryptHelper.AesDecrypt(data.RefreshToken);

                //var jwtAccessToken = tokenHandler.ReadToken(data.AccessToken) as JwtSecurityToken;
                //var jwtRefreshToken = tokenHandler.ReadToken(data.RefreshToken) as JwtSecurityToken;

                //data.AccessToken = CryptHelper.AesEncrypt(data.AccessToken);
                //data.RefreshToken = CryptHelper.AesEncrypt(data.RefreshToken);



                // ============================
                // SET AUTH COOKIE MVC
                // ============================
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, model.UserId),
                    new Claim(ClaimTypes.Name, data.UserName),
                    // custom claim
                    new Claim("PicturePath", data.PicturePath ?? "")

                };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = data.ExpiredAt
                    }
                );

                Response.Cookies.Append(_config["AppSettings:ApplicationID"] + "AccessToken", data.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = isHttpSecure,
                    Expires = data.ExpiredAt,
                    SameSite = isHttpSecure ? SameSiteMode.None : SameSiteMode.Lax
                });


                //Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
                //{
                //    HttpOnly = true,
                //    Secure = true,
                //    SameSite = SameSiteMode.Strict,
                //    Expires = DateTime.UtcNow.AddDays(7)
                //});
                Response.Cookies.Append(_config["AppSettings:ApplicationID"] + "RefreshToken", data.RefreshToken, new CookieOptions
                {
                    HttpOnly = false,
                    Secure = isHttpSecure,
                    Expires = DateTime.UtcNow.AddDays(7),
                    SameSite = isHttpSecure ? SameSiteMode.None : SameSiteMode.Lax
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                result.Data = ex.Message;
                return StatusCode(500, result);
            }

        }


        public async Task<IActionResult> Logout()
        {
            // Panggil API /api/auth/logout
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(new { }),
                "/api/auth/logout",
                HttpMethod.Post
            );
            if (result.Status)
            {
                Response.Headers.Remove("Cookie");
                Response.Cookies.Delete(_config["AppSettings:ApplicationID"] + "AccessToken");
                Response.Cookies.Delete(_config["AppSettings:ApplicationID"] + "RefreshToken");

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                // Redirect ke login
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // Logout gagal, bisa redirect atau tampil error
                TempData["Error"] = "Logout gagal, silakan coba lagi.";
                return RedirectToAction("Index", "Home");
            }
        }



    }

}
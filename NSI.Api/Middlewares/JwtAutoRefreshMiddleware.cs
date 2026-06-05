using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using NSI.Api.Auth;
//using NSI.Domain.Services;
using NSI.Infra.Services;

public class JwtAutoRefreshMiddleware
{
    private readonly RequestDelegate _next;

    public JwtAutoRefreshMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    //public async Task InvokeAsync(
    //    HttpContext context,
    //    JwtService jwtService,
    //    IRefreshTokenService refreshTokenService)
    //{
    //    var accessToken = context.Request.Cookies["access_token"];
    //    var refreshToken = context.Request.Cookies["refresh_token"];

    //    if (!string.IsNullOrEmpty(accessToken))
    //    {
    //        var handler = new JwtSecurityTokenHandler();

    //        try
    //        {
    //            handler.ValidateToken(
    //                accessToken,
    //                jwtService.GetTokenValidationParameters (),
    //                out _);
    //        }
    //        catch (SecurityTokenExpiredException)
    //        {
    //            // ⛔ access token expired → coba refresh
    //            if (!string.IsNullOrEmpty(refreshToken))
    //            {
    //                var stored = await refreshTokenService.ValidateAsync(refreshToken);

    //                if (stored != null)
    //                {
    //                    // ROTATE
    //                    await refreshTokenService.RotateAsync(stored);

    //                    var newJwt = jwtService.GenerateToken(
    //                        stored.UserId,
    //                        out var expiredAt);

    //                    var newRefresh =
    //                        await refreshTokenService.CreateAsync(stored.UserId);

    //                    context.Response.Cookies.Append(
    //                        "access_token",
    //                        newJwt,
    //                        new CookieOptions
    //                        {
    //                            HttpOnly = true,
    //                            Secure = true,
    //                            SameSite = SameSiteMode.Strict,
    //                            Expires = expiredAt
    //                        });

    //                    context.Response.Cookies.Append(
    //                        "refresh_token",
    //                        newRefresh,
    //                        new CookieOptions
    //                        {
    //                            HttpOnly = true,
    //                            Secure = true,
    //                            SameSite = SameSiteMode.Strict,
    //                            Expires = DateTime.UtcNow.AddDays(7)
    //                        });
    //                }
    //            }
    //        }
    //    }

    //    await _next(context);
    //}
}

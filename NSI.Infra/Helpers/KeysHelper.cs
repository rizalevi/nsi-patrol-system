using Microsoft.AspNetCore.Http;
using NM9Infra.Models.Global;

namespace NM9Infra.Helpers.Global
{
	public static class KeysHelper
	{
		public static string GetFailedLoginAttemptKey(AuthModel param, string clientIP)
		{
			return $"failed_login_attempt:{param.ApplicationID.ToLower()}:{clientIP}";
		}

		public static string GetUserMenuKey(AuthModel param)
		{
			return $"user_menu:{param.ApplicationID.ToLower()}:{param.UserID.ToLower()}::";
		}
		public static string GetUserMenuKey(AuthModel param, string clientIP, string userAgent) // sementara abaikan clientIp dan userAgent
		{
			userAgent = !string.IsNullOrEmpty(userAgent) ? CryptHelper.HashString(userAgent) : "";
			return $"user_menu:{param.ApplicationID.ToLower()}:{param.UserID.ToLower()}:{clientIP}:{userAgent}";
		}

		public static string GetRefreshTokenKey(AuthModel param, string clientIP, string userAgent)
		{
			userAgent = !string.IsNullOrEmpty(userAgent) ? CryptHelper.HashString(userAgent) : "";
			return $"refresh_token:{param.ApplicationID.ToLower()}:{param.UserID.ToLower()}:{clientIP}:{userAgent}";
		}

		public static string GetRateLimitUserKey(HttpRequest request)
		{
			// Kombinasi IP Address dan User-Agent sebagai kunci pengguna
			string userIp = request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
			string userAgent = request.Headers.UserAgent.ToString();
			return $"{userIp}:{userAgent}"; // Kombinasi IP dan User-Agent
		}

		public static string GetRateLimitCacheKey(string userKey)
		{
			return $"ratelimit_{userKey}";
		}

		//eof
	}
}
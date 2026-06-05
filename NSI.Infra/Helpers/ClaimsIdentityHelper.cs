using NSI.Domain.Models;
using System.Security.Claims;
using System.Security.Principal;

namespace NSI.Infra.Helpers
{
	public class ClaimsIdentityHelper
	{
		public static string? GetApplicationID(IPrincipal User)
		{
			return (User.Identity as ClaimsIdentity)?.FindFirst("ApplicationID")?.Value;
		}

		public static string? GetUserID(IPrincipal User)
		{
			return (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		}

		public static string? GetUserName(IPrincipal User)
		{
			return (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Name)?.Value;
		}

		public static string? GetRole(IPrincipal User)
		{
			return (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Role)?.Value;
		}

		public static string? GetEmail(IPrincipal User)
		{
			return (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Email)?.Value;
		}

		//komen dulu belum tau buat apa
		//public static AuthModel GetUserClaimsIdentity(IPrincipal User)
		//{
		//	ClaimsIdentity cid = (User.Identity as ClaimsIdentity) ?? new ClaimsIdentity();
		//	AuthModel userModel = new AuthModel
		//	{
		//		ApplicationID = cid.FindFirst("ApplicationID")?.Value ?? "",
		//		UserID = cid.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "",
		//		UserName = cid.FindFirst(ClaimTypes.Name)?.Value ?? "",
		//		Email = cid.FindFirst(ClaimTypes.Email)?.Value ?? "",
		//		Role = cid.FindFirst(ClaimTypes.Role)?.Value ?? ""
		//	};
		//	return userModel;
		//}

		//eof        
	}
}
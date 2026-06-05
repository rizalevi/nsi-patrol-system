using Microsoft.AspNetCore.Mvc;
using NSI.Application.Extensions;
using NSI.Domain.DTOs.Tasklist;
using NSI.Infra.Helpers;
using System.Security.Claims;

namespace NSI.Application.Services.Tasklist
{

    [ApiController, Route("service/notification")]
    public class NotificationServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;

        public NotificationServiceController(HttpRequestHelper htr)
        {
            _htr = htr;
        }


        // =========================
        // GET LIST NOTIFICATION
        // =========================
        [HttpGet, Route("list")]
        public async Task<IActionResult> GetList()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _htr.AsyncStringList<List<NotificationListDto>>(
                userId,
                "api/notification/list",
                HttpMethod.Get
            );
            return this.ResponseStatus(result);
            //return Json(new {
            //    status = result.Status,
            //    message = result.Message,
            //    data = result.Data });
        }

        // =========================
        // GET UNREAD COUNT
        // =========================
        [HttpGet, Route("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _htr.AsyncAsResponseModel<NotificationCountDto>(
                userId,
                "api/notification/unread-count",
                HttpMethod.Get
            );
            return this.ResponseStatus(result);
            //return Json(new { success = result.Status, message = result.Message });
        }

        // =========================
        // MARK ALL READ
        // =========================
        [HttpPost, Route("mark-all-read")]
        public async Task<IActionResult> MarkAllRead()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _htr.AsyncAsResponseModel<object>(
                userId,
                "api/notification/mark-all-read",
                HttpMethod.Post
            );

            return this.ResponseStatus(result);
        }

    }
}

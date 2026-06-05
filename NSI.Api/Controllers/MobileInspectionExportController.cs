using Microsoft.AspNetCore.Mvc;
using NSI.Api.Repositories.Mobile;
using NSI.Api.Services;
using NSI.Domain.DTOs.Mobile;
using NSI.Domain.Interfaces.Tasklist;

namespace NSI.Api.Controllers
{
    [Route("api/mobile")]
    [ApiController]
    public class MobileInspectionExportController : CommonController
    {
        private readonly MobileInspectionExportRepositories _service;
        //private readonly NotificationController _notification;
        private readonly INotificationRepository _repo;
        private readonly NotificationService _signalR;

        public MobileInspectionExportController( MobileInspectionExportRepositories service, 
                                                 INotificationRepository repo, 
                                                 NotificationService signalR)
        {
            _service = service;
            _repo = repo;
            _signalR = signalR;
            //_notification = notification;
        }

        [HttpPost("syncexport")]
        public async Task<IActionResult> SyncExport(
        [FromBody] MobileInspectionExportDto request)
        {
            if (request.Headers.Count == 0)
                return BadRequest("No data");

            try
            {
                var result = await _service.SyncExport(request);
                var userId = User.Identity?.Name;
                if (result)
                {
                    var notif = await _repo.SendNotifExportData(request, userId);

                    //--------------------------------------------------------
                    // LOOP ALL AFFECTED USERS
                    //--------------------------------------------------------
                    foreach (var item in notif)
                    {
                        //----------------------------------------------------
                        // item.UserId = target receiver
                        //----------------------------------------------------
                        var unreadCount = await _repo.GetUnreadCount(item.UserId);

                        //var unreadCount = await _repo.GetUnreadCount(userId);
                        var listData = await _repo.GetList(item.UserId);

                        // Push realtime
                        await _signalR.PushNewNotificationAsync(item.UserId, listData);
                        await _signalR.UpdateCount(item.UserId, unreadCount);
                    }



                }

                return Ok(new
                {
                    success = result,
                    message = $"Processed {request.Headers.Count} data"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        //[HttpPost("syncexport")]
        //public async Task<IActionResult> SyncExport([FromBody] MobileInspectionExportDto request)
        //{
        //    if (request.Headers.Count == 0)
        //        return BadRequest("No data");
        //    try
        //    {
        //        var result = await _service.SyncExport(request);
        //        //push notif
        //        if (result==true)
        //        {
        //            await _notification.ExportData(request);
        //        }

        //        return Ok(new
        //        {
        //            success = result,
        //            message = $"Processed {request.Headers.Count} data"
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new
        //        {
        //            success = false,
        //            message = ex.Message
        //        });
        //    }
        //}
    }
}

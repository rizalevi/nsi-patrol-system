using Google.Apis.Storage.v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Api.Services;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.DTOs.Mobile;
using NSI.Domain.Interfaces.Tasklist;
using NSI.Domain.Models.Inspection;
using NSI.Domain.Models.Tasklist;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/notification")]
    public class NotificationController : CommonController
    {
        private readonly INotificationRepository _repo;
        private readonly NotificationService _signalR;

        public NotificationController(INotificationRepository repo, NotificationService signalR)
        {
            _repo = repo;
            _signalR = signalR;
        }



        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var userId = User.Identity?.Name;
            return Success(await _repo.GetList(userId));
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetCount()
        {
            var userId = User.Identity?.Name;
            return Success(new { count = await _repo.GetUnreadCount(userId) });
        }

        [HttpPost("mark-all-read")]
        public async Task<IActionResult> MarkAllRead()
        {
            var userId = User.Identity?.Name;

            await _repo.MarkAllAsRead(userId);
            await _signalR.MarkAsRead(userId);

            await _signalR.UpdateCount(userId, 0);

            return Success();
        }


        [HttpPost("export-data")]
        public async Task ExportData([FromBody] MobileInspectionExportDto payload)
        {
            //save
            var userId = User.Identity?.Name;
            var result = await _repo.SendNotifExportData(payload, userId);

            //--------------------------------------------------------
            // LOOP ALL AFFECTED USERS
            //--------------------------------------------------------
            foreach (var item in result)
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

        [HttpPost("approve-inspector-pushnotif")]
        public async Task InspectorApproveBatch([FromBody] string userID) //(InspectionDataUploadParamDto payload)
        {
            //save
            //var userId = User.Identity?.Name;
            //await _repo.SendNotifInspectorBatch(payload, userId);

            var unreadCount = await _repo.GetUnreadCount(userID);
            var listData = await _repo.GetList(userID);

            // Push realtime
            await _signalR.PushNewNotificationAsync(userID, listData);
            await _signalR.UpdateCount(userID, unreadCount);
        }

        [HttpPost("inspector-submit-detail-pushnotif")]
        public async Task InspectorSubmitSendBack([FromBody] string userID)//[FromBody] InspectionDataUploadUpdateDto payload)
        {
            //save
            //var userId = User.Identity?.Name;
            //await _repo.SendNotifInspectorSubmitSendBack(payload, userId);

            //get new data
            var unreadCount = await _repo.GetUnreadCount(userID);
            var listData = await _repo.GetList(userID);

            // Push realtime
            await _signalR.PushNewNotificationAsync(userID, listData);
            await _signalR.UpdateCount(userID, unreadCount);

        }

        //[HttpPost("leader-batch-pushnotif")]
        //public async Task CheckLeaderApproveBatch(InspectionDataLeader payload)
        //{
        //    //save
        //    //var userId = User.Identity?.Name;
        //    //await _repo.SendNotifCheckLeaderBatch(payload, userId);

        //    //get new data
        //    var unreadCount = await _repo.GetUnreadCount(userID);
        //    var listData = await _repo.GetList(userID);

        //    // Push realtime
        //    await _signalR.PushNewNotificationAsync(userID, listData);
        //    await _signalR.UpdateCount(userID, unreadCount);
        //}

        [HttpPost("leader-batch-pushnotif")]
        public async Task CheckLeaderApproveBatch([FromBody] string userIDs)
        {
            if (string.IsNullOrWhiteSpace(userIDs))
                return;

            //--------------------------------------------------------
            // SPLIT USER TARGET
            //--------------------------------------------------------
            var users = userIDs
                .Split('|', StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToList();

            //--------------------------------------------------------
            // LOOP PUSH REALTIME
            //--------------------------------------------------------
            foreach (var userId in users)
            {
                try
                {
                    //------------------------------------------------
                    // GET DATA USER
                    //------------------------------------------------
                    var unreadCount =
                        await _repo.GetUnreadCount(userId);

                    var listData =
                        await _repo.GetList(userId);

                    //------------------------------------------------
                    // PUSH REALTIME
                    //------------------------------------------------
                    await _signalR.PushNewNotificationAsync(
                        userId,
                        listData
                    );

                    //------------------------------------------------
                    // UPDATE BADGE
                    //------------------------------------------------
                    await _signalR.UpdateCount(
                        userId,
                        unreadCount
                    );
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex,
                    //    $"Push notif failed for user {userId}");
                }
            }
        }

        [HttpPost("leader-detail-pushnotif")]
        public async Task CheckLeaderSubmitDetail([FromBody] string userID)
        {
           
            //save
           // var userId = result.TargetUser;// User.Identity?.Name;
            //await _repo.SendNotifCheckLeaderDetail(payload, userId);

            //get new data
            var unreadCount = await _repo.GetUnreadCount(userID);
            var listData = await _repo.GetList(userID);

            // Push realtime
            await _signalR.PushNewNotificationAsync(userID, listData);
            await _signalR.UpdateCount(userID, unreadCount);
        }

        /*belum kepake karena setelah approve supervisor tidak perlu ada notif apa-apa*/

        [HttpPost("supervisor-batch")]
        public async Task SupervisorApproveBatch(InspectionDataApprove payload)
        {
            //save
            var userId = User.Identity?.Name;
            //await _repo.SendNotifSupervisorBatch(payload, userId);

            //get new data
            var unreadCount = await _repo.GetUnreadCount(userId);
            var listData = await _repo.GetList(userId);

            // Push realtime
            await _signalR.PushNewNotificationAsync(userId, listData);
            await _signalR.UpdateCount(userId, unreadCount);
        }

        [HttpPost("supervisor-submit")]
        public async Task CheckSupervisorSubmitDetail(InspectionDataApproveDetail payload)
        {
            //save
            var userId = User.Identity?.Name;
            //await _repo.SendNotifSupervisorDetail(payload, userId);

            //get new data
            var unreadCount = await _repo.GetUnreadCount(userId);
            var listData = await _repo.GetList(userId);

            // Push realtime
            await _signalR.PushNewNotificationAsync(userId, listData);
            await _signalR.UpdateCount(userId, unreadCount);
        }


    }
}

using Microsoft.AspNetCore.SignalR;
using NSI.Api.Hubs;
using NSI.Domain.DTOs.Tasklist;

namespace NSI.Api.Services
{
    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> _hub;

        public NotificationService(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        //public async Task SendToUser(string userId, object data)
        //{
        //    await _hub.Clients.Group(userId)
        //        .SendAsync("ReceiveNotification", data);    
        //}

        //--------------------------------------------------------
        // PUSH NEW NOTIFICATION
        //--------------------------------------------------------
        public async Task PushNewNotificationAsync( string userId, object notification)
        {
            await _hub.Clients
                .Group($"user:{userId}")
                .SendAsync(
                    "ReceiveNotification",
                    notification
                );
        }

        //--------------------------------------------------------
        // UPDATE BADGE COUNT
        //--------------------------------------------------------
        public async Task UpdateCount(string userId, int count)
        {
            await _hub.Clients
                .Group($"user:{userId}")
                .SendAsync(
                    "NotificationCountUpdated",
                    count
                );
        }

        //public async Task UpdateCount(string userId, int count)
        //{
        //    await _hub.Clients.Group(userId)
        //        .SendAsync("NotificationCountUpdated", count);
        //}

        public async Task MarkAsRead(string userId)
        {
            await _hub.Clients.Group(userId)
                .SendAsync("NotificationsMarkedRead");
        }
    }
}

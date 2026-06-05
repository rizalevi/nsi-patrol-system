using Microsoft.AspNetCore.SignalR;


namespace NSI.Api.Hubs
{
    public class NotificationHub : Hub
    {

        //public override async Task OnConnectedAsync()
        //{
        //    var userId = Context.User?.Identity?.Name;

        //    if (!string.IsNullOrEmpty(userId))
        //    {
        //        await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        //    }

        //    await base.OnConnectedAsync();
        //}
        // saat user connect
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.Identity?.Name;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                    $"user:{userId}"
                );

                Console.WriteLine(
                    $"JOIN GROUP => user:{userId}"
                );
            }

            await base.OnConnectedAsync();
        }

        // saat user disconnect
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.Identity?.Name;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                await Groups.RemoveFromGroupAsync(
                    Context.ConnectionId,
                    $"user:{userId}"
                );

                Console.WriteLine(
                    $"LEAVE GROUP => user:{userId}"
                );
            }

            await base.OnDisconnectedAsync(exception);
        }

        //public override async Task OnDisconnectedAsync(Exception? exception)
        //{
        //    var userId = Context.User?.Identity?.Name;

        //    if (!string.IsNullOrEmpty(userId))
        //    {
        //        await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        //    }

        //    await base.OnDisconnectedAsync(exception);
        //}
    }
}

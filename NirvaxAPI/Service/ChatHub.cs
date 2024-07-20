
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebAPI.Service
{
    public class ChatHub : Hub
    {
        public async Task SendMessageToRoom(string roomId, string senderId, string content)
        {
            await Clients.Group(roomId).SendAsync("ReceiveMessage", senderId, content);
        }

        public override async Task OnConnectedAsync()
        {
            var roomId = Context.GetHttpContext().Request.Query["roomId"];
            if (!string.IsNullOrEmpty(roomId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var roomId = Context.GetHttpContext().Request.Query["roomId"];
            if (!string.IsNullOrEmpty(roomId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }


}

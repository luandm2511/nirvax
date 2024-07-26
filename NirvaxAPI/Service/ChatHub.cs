
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebAPI.Service
{
    public class ChatHub : Hub
    {
        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        public async Task LeaveRoom(string roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        }

        public async Task SendMessageToRoom(string roomId, string senderId, string message)
        {
            await Clients.Group(roomId).SendAsync("ReceiveMessage", senderId, message);
        }
    }

}


using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebAPI.Service
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(int roomId, string user, string message)
        {
            // Gửi tin nhắn tới tất cả các client trong phòng chat
            await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", user, message);
        }

        public async Task JoinRoom(int roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
        }

        public async Task LeaveRoom(int roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
        }
    }

}

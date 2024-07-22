using Microsoft.AspNetCore.SignalR;

namespace WebAPI.Service
{
    public class NotificationHub : Hub
    {
        // Thêm người dùng vào nhóm dựa trên loại người dùng
        public async Task AddToGroup(string userId, string userType)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{userType}-{userId}");
        }

        // Xóa người dùng khỏi nhóm khi họ ngắt kết nối
        public async Task RemoveFromGroup(string userId, string userType)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"{userType}-{userId}");
        }

        // Phương thức gửi thông báo đến nhóm người dùng
        public async Task SendNotification(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveNotification", message);
        }
    }
}

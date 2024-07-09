using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;

namespace DataAccess.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationDAO _noti;
        public NotificationRepository(NotificationDAO noti)
        {
            _noti = noti;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _noti.AddNotificationAsync(notification);
        }

        public async Task<Notification> GetNotificationByidAsync(int id)
        {
            return await _noti.GetNotificationByidAsync(id);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByOwnerAsync(int id)
        {
            return await _noti.GetNotificationsByOwnerAsync(id);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserAsync(int id)
        {
            return await _noti.GetNotificationsByUserAsync(id);
        }
    }
}

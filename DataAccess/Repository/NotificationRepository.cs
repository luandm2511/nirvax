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
        public async Task<bool> AddNotificationAsync(Notification notification) => await NotificationDAO.AddNotificationAsync(notification);

        public async Task<Notification> GetNotificationByidAsync(int id)
        {
            return await NotificationDAO.GetNotificationByidAsync(id);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByOwnerAsync(int id)
        {
            return await NotificationDAO.GetNotificationsByOwnerAsync(id);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserAsync(int id)
        {
            return await NotificationDAO.GetNotificationsByUserAsync(id);
        }
    }
}

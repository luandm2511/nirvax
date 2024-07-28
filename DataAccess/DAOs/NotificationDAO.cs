using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAOs
{
    public class NotificationDAO
    {
        private readonly NirvaxContext _context;

        public NotificationDAO(NirvaxContext context)
        {
            _context = context;
        }
        public async Task AddNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<string> UpdateStatusNotificationAsync(Notification notification)
        {
            notification.IsRead = true;
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return notification.Url;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserAsync(int id)
        {
            return await _context.Notifications
                .Where(n => n.AccountId == id)
                .OrderByDescending(n => n.NotificationId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByOwnerAsync(int id)
        {
            return await _context.Notifications
                .Where(n => n.OwnerId == id)
                .OrderByDescending(n => n.NotificationId)
                .ToListAsync();
        }

        public async Task<Notification> GetNotificationByidAsync(int id)
        {
            return await _context.Notifications
                .Include(n => n.Owner)
                .Include(n => n.Account)
                .FirstOrDefaultAsync(n => n.NotificationId == id);
        }
    }
}

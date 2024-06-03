using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAOs
{
    public static class NotificationDAO
    {
        private static readonly NirvaxContext _context = new NirvaxContext();
        public static async Task<bool> AddNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        public static async Task<IEnumerable<Notification>> GetNotificationsByUserAsync(int id)
        {
            return await _context.Notifications
                .Include(n => n.Owner)
                .Include(n => n.Account)
                .Where(n => n.AccountId == id)
                .ToListAsync();
        }

        public static async Task<IEnumerable<Notification>> GetNotificationsByOwnerAsync(int id)
        {
            return await _context.Notifications
                .Include(n => n.Owner)
                .Include(n => n.Account)
                .Where(n => n.OwnerId == id)
                .ToListAsync();
        }

        public static async Task<Notification> GetNotificationByidAsync(int id)
        {
            return await _context.Notifications
                .Include(n => n.Owner)
                .Include(n => n.Account)
                .FirstOrDefaultAsync(n => n.AccountId == id);
        }
    }
}

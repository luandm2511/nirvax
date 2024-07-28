using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace DataAccess.IRepository
{
    public interface INotificationRepository
    {
        Task AddNotificationAsync(Notification notification);
        Task<IEnumerable<Notification>> GetNotificationsByUserAsync(int id);
        Task<IEnumerable<Notification>> GetNotificationsByOwnerAsync(int id);
        Task<Notification> GetNotificationByidAsync(int id);
        Task<string> UpdateStatusNotificationAsync(Notification notification);
    }
}

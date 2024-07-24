using BusinessObject.Models;

namespace WebAPI.Service
{
    public interface IAccessLogService
    {
        Task LogAccessAsync(HttpContext context);
        Task<IEnumerable<AccessLog>> GetAccessLogsAsync();
        Task<int> GetAccessCountAsync(DateTime startDate, DateTime endDate);
    }
}

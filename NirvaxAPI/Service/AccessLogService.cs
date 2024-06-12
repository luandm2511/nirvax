using BusinessObject.Models;
using DataAccess.IRepository;

namespace WebAPI.Service
{
    public class AccessLogService : IAccessLogService
    {
        private readonly IAccessLogRepository _accessLogRepository;

        public AccessLogService(IAccessLogRepository accessLogRepository)
        {
            _accessLogRepository = accessLogRepository;
        }

        public async Task<bool> LogAccessAsync(HttpContext context)
        {
            var accessLog = new AccessLog
            {
                AccessTime = DateTime.UtcNow,
                IpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                UserAgent = context.Request.Headers["User-Agent"].ToString()
            };

            await _accessLogRepository.LogAccessAsync(accessLog);
            return true;
        }

        public async Task<IEnumerable<AccessLog>> GetAccessLogsAsync()
        {
            return await _accessLogRepository.GetAccessLogsAsync();
        }

        public async Task<int> GetAccessCountAsync(DateTime startDate, DateTime endDate)
        {
            return await _accessLogRepository.GetAccessCountAsync(startDate, endDate);
        }
    }
}

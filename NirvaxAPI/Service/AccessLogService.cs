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

        public async Task LogAccessAsync(HttpContext context)
        {
            var accessLog = new AccessLog
            {
                AccessTime = DateTime.UtcNow,
                IpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                UserAgent = context.Request.Headers["User-Agent"].ToString()
            };

            await _accessLogRepository.LogAccessAsync(accessLog);
        }
    }
}

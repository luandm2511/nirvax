using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Service;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccessLogController : ControllerBase
    {
        private readonly IAccessLogService _accessLogService;

        public AccessLogController(IAccessLogService accessLogService)
        {
            _accessLogService = accessLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccessLogs()
        {
            var logs = await _accessLogService.GetAccessLogsAsync();
            return Ok(logs);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetAccessCount(DateTime startDate, DateTime endDate)
        {
            var count = await _accessLogService.GetAccessCountAsync(startDate, endDate);
            return Ok(count);
        }
    }

}

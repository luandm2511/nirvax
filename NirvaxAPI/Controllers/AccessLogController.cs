using DataAccess.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Service;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    public class AccessLogController : ControllerBase
    {
        private readonly IAccessLogRepository _accessLogRepository;

        public AccessLogController(IAccessLogRepository accessLogRepository)
        {
            _accessLogRepository = accessLogRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccessLogs()
        {
            var logs = await _accessLogRepository.GetAccessLogsAsync();
            return Ok(logs);
        }
    }

}

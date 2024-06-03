using AutoMapper;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUser(int id)
        {
            try
            {
                var noti = _notificationRepository.GetNotificationsByUserAsync(id);
                return Ok(noti);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("owner/{id}")]
        public async Task<IActionResult> GetByOwner(int id)
        {
            try
            {
                var noti = _notificationRepository.GetNotificationsByOwnerAsync(id);
                return Ok(noti);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByid(int id)
        {
            try
            {
                var noti = _notificationRepository.GetNotificationByidAsync(id);
                if(noti == null)
                {
                    return NotFound(new { message = "Notification is not found." });
                }
                return Ok(noti);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
    }
}

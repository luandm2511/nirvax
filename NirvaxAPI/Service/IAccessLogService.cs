using BusinessObject.Models;

namespace WebAPI.Service
{
    public interface IAccessLogService
    {
        Task LogAccessAsync(HttpContext context);
    }
}

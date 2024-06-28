using WebAPI.Service;

namespace WebAPI.Helpers
{
    public class AccessLogMiddleware
    {
        private readonly RequestDelegate _next;

        public AccessLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Lấy IAccessLogService từ các dịch vụ scoped của request
            var accessLogService = context.RequestServices.GetRequiredService<IAccessLogService>();

            await accessLogService.LogAccessAsync(context);
            await _next(context);
        }
    }
}

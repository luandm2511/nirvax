namespace WebAPI.Helpers
{
    public static class AccessLogMiddlewareExtensions
    {
        public static IApplicationBuilder UseAccessLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AccessLogMiddleware>();
        }
    }
}

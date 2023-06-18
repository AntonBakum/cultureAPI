namespace CultureAPI.Infrastructure.Middleware.LoggingMiddleware
{
    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(
        this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}

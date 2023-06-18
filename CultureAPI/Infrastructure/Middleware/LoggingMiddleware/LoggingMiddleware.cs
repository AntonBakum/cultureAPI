namespace CultureAPI.Infrastructure.Middleware.LoggingMiddleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            finally
            {
                _logger.LogInformation($"{DateTime.Now} ; {context.Request?.Method} ; " +
                    $"{context.Request?.Path.Value} ; {context.Response?.StatusCode}");
            }
        }
    }
}

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CultureAPI.Infrastructure.Logging
{
    public static class FileLoggerExtensions
    {
        private static string CreatePath()
        {
            string filePattern = "file-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            return Path.Combine(Environment.CurrentDirectory, $"Logs\\{filePattern}");
        }
        public static ILoggingBuilder AddFileLoggerProvider(this ILoggingBuilder builder)
        {
            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, FileLoggerProvider>(
                    (srv) =>
                    {
                        return new FileLoggerProvider(CreatePath());
                    }
             ));
            return builder;
        }
    }
}

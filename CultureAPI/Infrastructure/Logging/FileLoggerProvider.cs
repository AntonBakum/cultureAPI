using System.Collections.Concurrent;

namespace CultureAPI.Infrastructure.Logging
{
    public class FileLoggerProvider: ILoggerProvider
    {
        private readonly string path;
        public FileLoggerProvider(string path)
        {
            this.path = path;
        }

        private readonly ConcurrentDictionary<string, ILogger> _loggers =
            new ConcurrentDictionary<string, ILogger>(StringComparer.OrdinalIgnoreCase);
        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, key => new FileLogger(path));
        }

        public void Dispose()
        {

        }
    }
}

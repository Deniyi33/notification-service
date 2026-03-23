using Microsoft.Extensions.Logging;

namespace Feex.Infrastructure.Logger
{
    public interface ILog<T>
    {
        void Log(string message, LogLevel logType = LogLevel.Information);
        void Log(object data, string message = "", LogLevel logType = LogLevel.Information);
        void Log(string message, LogLevel logType = LogLevel.Information, params object[] args);
        void Log(Exception ex, LogLevel logType = LogLevel.Information);
    }

}

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EMI.Infrastructure.Logger
{

    public class LogService<T> : ILog<T>
    {
        private readonly ILogger<T> _logger;

        public LogService(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void Log(string message, LogLevel logType = LogLevel.Information)
        {
            LogInternal(logType, log => log.WithClassAndMethodNames<T>().Log(logType, message));
        }

        public void Log(object data, string message = "", LogLevel logType = LogLevel.Information)
        {
            var serializedData = JsonConvert.SerializeObject(data);
            LogInternal(logType, log => log.WithClassAndMethodNames<T>().Log(logType, $"{message} {serializedData}"));
        }

        public void Log(string message, LogLevel logType = LogLevel.Information, params object[] args)
        {
            LogInternal(logType, log => log.WithClassAndMethodNames<T>().Log(logType, message, args));
        }

        public void Log(Exception ex, LogLevel logType = LogLevel.Information)
        {
            var serializedException = JsonConvert.SerializeObject(ex);
            LogInternal(logType, log => log.WithClassAndMethodNames<T>().Log(logType, ex, $"EXCEPTION {serializedException}"));
        }

        private void LogInternal(LogLevel logType, Action<ILogger> logAction)
        {
            logAction(_logger);
        }
    }

    public static class LoggerExtensions
    {
        public static ILogger WithClassAndMethodNames<T>(this ILogger logger, [CallerMemberName] string methodName = "")
        {
            var className = typeof(T).Name;

            logger.BeginScope(new Dictionary<string, object>
            {
                ["ClassName"] = $"[{className}]::",
                ["MethodName"] = $"[{methodName}]"
            });

            return logger;
        }
    }


}

using System;
using System.Globalization;
using Microsoft.Extensions.Logging;

using NLogger = NLog;

namespace Es.Extensions.Logging.NLog
{
    /// <summary>
    ///  Implement NLog's Logger in a Microsoft.Extensions.Logging's interface <see cref="Microsoft.Extensions.Logging.ILogger"/>.
    /// </summary>
    internal class Logger : ILogger
    {
        private NLogger.Logger _logger;

        public Logger(NLogger.Logger logger) {
            _logger = logger;
        }

        public IDisposable BeginScope<TState>(TState state) {
            if (state == null) {
                throw new ArgumentNullException(nameof(state));
            }
            return NLogger.NestedDiagnosticsContext.Push(state);
        }

        public bool IsEnabled(LogLevel logLevel) {
            return _logger.IsEnabled(GetLogLevel(logLevel));
        }

        private bool IsEnabled(NLogger.LogLevel logLevel) {
            return _logger.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
            var nLogLevel = GetLogLevel(logLevel);

            if (IsEnabled(nLogLevel)) {

                if (formatter == null) {
                    throw new ArgumentNullException(nameof(formatter));
                }

                var message = formatter(state, exception);

                if (string.IsNullOrEmpty(message))
                    return;

                var eventInfo = NLogger.LogEventInfo.Create(
                      nLogLevel,
                      _logger.Name,
                      exception,
                      CultureInfo.CurrentCulture,
                      message);
                eventInfo.Properties["EventId.Id"] = eventId.Id;
                eventInfo.Properties["EventId.Name"] = eventId.Name;
                eventInfo.Properties["EventId"] = eventId;
                _logger.Log(eventInfo);
            }
        }

        private NLogger.LogLevel GetLogLevel(LogLevel logLevel) {
            switch (logLevel) {
                case LogLevel.Trace: return NLogger.LogLevel.Trace;
                case LogLevel.Debug: return NLogger.LogLevel.Debug;
                case LogLevel.Information: return NLogger.LogLevel.Info;
                case LogLevel.Warning: return NLogger.LogLevel.Warn;
                case LogLevel.Error: return NLogger.LogLevel.Error;
                case LogLevel.Critical: return NLogger.LogLevel.Fatal;
                case LogLevel.None: return NLogger.LogLevel.Off;
                default: return NLogger.LogLevel.Debug;
            }
        }
    }
}
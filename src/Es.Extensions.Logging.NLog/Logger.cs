using System;
using System.Globalization;
using Microsoft.Extensions.Logging;

using NLogger = NLog;

namespace Es.Extensions.Logging.NLog
{
    /// <summary>
    ///  Implement NLog's Logger in a Microsoft.Extensions.Logging's interface <see cref="Microsoft.Extensions.Logging.ILogger"/>.
    ///  https://github.com/NLog/NLog.Extensions.Logging/blob/master/src/NLog.Extensions.Logging/NLogLogger.cs
    /// </summary>
    internal class Logger : ILogger
    {
        private NLogger.Logger _logger;
        private readonly NLogProviderOptions _options;

        private static readonly object _emptyEventId = default(EventId);    // Cache boxing of empty EventId-struct
        private static readonly object _zeroEventId = default(EventId).Id;  // Cache boxing of zero EventId-Value
        private Tuple<string, string, string> _eventIdPropertyNames;


        public Logger(NLogger.Logger logger, NLogProviderOptions options = null)
        {
            _logger = logger;
            _options = options ?? NLogProviderOptions.Default;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }
            return NLogger.NestedDiagnosticsContext.Push(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(GetLogLevel(logLevel));
        }

        private bool IsEnabled(NLogger.LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var nLogLevel = GetLogLevel(logLevel);

            if (IsEnabled(nLogLevel))
            {
                if (formatter == null)
                {
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

                if (!_options.IgnoreEmptyEventId || eventId.Id != 0 || !string.IsNullOrEmpty(eventId.Name))
                {
                    var eventIdPropertyNames = _eventIdPropertyNames ?? new Tuple<string, string, string>(null, null, null);
                    var eventIdSeparator = _options.EventIdSeparator ?? string.Empty;
                    if (!ReferenceEquals(eventIdPropertyNames.Item1, eventIdSeparator))
                    {
                        // Perform atomic cache update of the string-allocations matching the current separator
                        eventIdPropertyNames = new Tuple<string, string, string>(
                            eventIdSeparator,
                            string.Concat("EventId", eventIdSeparator, "Id"),
                            string.Concat("EventId", eventIdSeparator, "Name"));
                        _eventIdPropertyNames = eventIdPropertyNames;
                    }

                    eventInfo.Properties[eventIdPropertyNames.Item2] = eventId.Id == 0 ? _zeroEventId : eventId.Id;
                    eventInfo.Properties[eventIdPropertyNames.Item3] = eventId.Name;
                    eventInfo.Properties["EventId"] = (eventId.Id == 0 && eventId.Name == null) ? _emptyEventId : eventId;
                }
                _logger.Log(eventInfo);
            }
        }

        private NLogger.LogLevel GetLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
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
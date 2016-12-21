using Microsoft.Extensions.Logging;
using NLogger = NLog;

namespace Es.Extensions.Logging.NLog
{
    /// <summary>
    /// Provider logger for NLog.
    /// </summary>
    public class NLogLoggerProvider : ILoggerProvider
    {
        private readonly NLogger.LogFactory _factory;
        private bool _disposed = false;

        /// <summary>
        /// <see cref="NLogLoggerProvider"/> with default LogManager.
        /// </summary>
        public NLogLoggerProvider() {
        }


        /// <summary>
        /// <see cref="NLogLoggerProvider"/> with default LogFactory.
        /// </summary>
        /// <param name="logFactory"><see cref="NLogger.LogFactory"/></param>
        public NLogLoggerProvider(NLogger.LogFactory logFactory) {
            _factory = logFactory;
        }

        /// <summary>
        /// Create a logger with the name <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of the logger to be created.</param>
        /// <returns>New Logger</returns>
        public ILogger CreateLogger(string name) {
            if (_factory == null) {
                //usage XmlLoggingConfiguration
                //e.g LogManager.Configuration = new XmlLoggingConfiguration(fileName, true);
                return new Logger(NLogger.LogManager.GetLogger(name));
            }
            return new Logger(_factory.GetLogger(name));
        }

        /// <summary>
        /// Cleanup
        /// </summary>
        public void Dispose() {
            if (_factory != null && !_disposed) {
                _factory.Flush();
                _factory.Dispose();
                _disposed = true;
            }
        }
    }
}
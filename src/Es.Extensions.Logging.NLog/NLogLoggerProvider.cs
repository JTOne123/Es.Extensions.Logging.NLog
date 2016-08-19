using Microsoft.Extensions.Logging;
using NLogger = NLog;

namespace Es.Extensions.Logging.NLog
{
    public class NLogLoggerProvider : ILoggerProvider
    {
        private readonly NLogger.LogFactory _factory;
        private bool _disposed = false;

        public NLogLoggerProvider() {
        }

        public NLogLoggerProvider(NLogger.LogFactory logFactory) {
            _factory = logFactory;
        }

        public ILogger CreateLogger(string name) {
            if (_factory == null) {
                //usage XmlLoggingConfiguration
                //e.g LogManager.Configuration = new XmlLoggingConfiguration(fileName, true);
                return new Logger(NLogger.LogManager.GetLogger(name));
            }
            return new Logger(_factory.GetLogger(name));
        }

        public void Dispose() {
            if (_factory != null && !_disposed) {
                _factory.Flush();
                _factory.Dispose();
                _disposed = true;
            }
        }
    }
}
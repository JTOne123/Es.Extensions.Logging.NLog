using System.IO;
using System.Reflection;
using Es.Extensions.Logging.NLog;
using Microsoft.AspNetCore.Hosting;
using NLogger = NLog;

namespace Microsoft.Extensions.Logging
{
    public static class NLogLoggerFactoryExtensions
    {
        public static ILoggerFactory AddNLog(this ILoggerFactory factory, NLogger.LogFactory logFactory) {
            factory.AddProvider(new NLogLoggerProvider(logFactory));
            return factory;
        }

        public static ILoggerFactory AddNLog(this ILoggerFactory factory) {
            NLogger.LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging")));
            NLogger.LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging.Abstractions")));
            NLogger.LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging.Console")));
            NLogger.LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging.Debug")));
            NLogger.LogManager.AddHiddenAssembly(typeof(Logger).GetTypeInfo().Assembly);

            using (var provider = new NLogLoggerProvider()) {
                factory.AddProvider(provider);
            }
            return factory;
        }

        public static void ConfigureNLog(this IHostingEnvironment env, string configFileRelativePath) {
            var fileName = Path.Combine(env.ContentRootPath, configFileRelativePath);
            ConfigureNLog(fileName);
        }

        private static void ConfigureNLog(string fileName) {
            NLogger.LogManager.Configuration = new NLogger.Config.XmlLoggingConfiguration(fileName, true);
        }
    }
}
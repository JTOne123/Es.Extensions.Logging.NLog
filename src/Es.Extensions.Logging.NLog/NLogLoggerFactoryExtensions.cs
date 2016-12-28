using System;
using System.IO;
using System.Reflection;
using Es.Extensions.Logging.NLog;
using Microsoft.AspNetCore.Hosting;
using NLogger = NLog;

namespace Microsoft.Extensions.Logging
{
    /// <summary>
    /// NLogLoggerFactoryExtensions
    /// </summary>
    public static class NLogLoggerFactoryExtensions
    {
        /// <summary>
        /// Enable NLog as logging provider in ASP.NET Core.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="logFactory"><see cref="NLogger.LogFactory"/></param>
        /// <returns></returns>
        public static ILoggerFactory AddNLog(this ILoggerFactory factory, NLogger.LogFactory logFactory) {
            factory.AddProvider(new NLogLoggerProvider(logFactory));
            return factory;
        }

        /// <summary>
        /// Enable NLog as logging provider in ASP.NET Core.
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Apply NLog configuration from XML config.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="configFileRelativePath">relative path to NLog configuration file.</param>
        public static void ConfigureNLog(this IHostingEnvironment env, string configFileRelativePath) {
            var fileName = Path.Combine(env.ContentRootPath, configFileRelativePath);
            ConfigureNLog(fileName);
        }

        private static void ConfigureNLog(string fileName) {
            NLogger.LogManager.Configuration = new NLogger.Config.XmlLoggingConfiguration(fileName, true);
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        public static void Error(this ILogger logger, string message) {
            logger.LogError(0, message);
        }

        /// <summary>
        /// Errors the specified format.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void Error(this ILogger logger, string format, params object[] args) {
            logger.LogError(0, format, args);
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        /// <param name="error">The error.</param>
        public static void Error(this ILogger logger, string message, Exception error) {
            logger.LogError(0, error, message);
        }

        /// <summary>
        /// Errors the specified error.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="error">The error.</param>
        public static void Error(this ILogger logger, Exception error) {
            logger.LogError(0, error, error.Message);
        }
    }
}
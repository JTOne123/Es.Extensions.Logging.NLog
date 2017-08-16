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
        /// <param name="factory"><see cref="ILoggerFactory"/></param>
        /// <param name="logFactory"><see cref="NLogger.LogFactory"/></param>
        /// <param name="options"><see cref="NLogProviderOptions"/></param>
        /// <returns></returns>
        public static ILoggerFactory AddNLog(this ILoggerFactory factory, NLogger.LogFactory logFactory, NLogProviderOptions options = null)
        {
            factory.AddProvider(new NLogLoggerProvider(logFactory, options));
            return factory;
        }

        /// <summary>
        /// Enable NLog as logging provider in ASP.NET Core.
        /// </summary>
        /// <param name="factory"><see cref="ILoggerFactory"/></param>
        /// <param name="options"><see cref="NLogProviderOptions"/></param>
        /// <returns></returns>
        public static ILoggerFactory AddNLog(this ILoggerFactory factory, NLogProviderOptions options = null)
        {
            NLogger.LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging")));
            NLogger.LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging.Abstractions")));
            try
            {
                var filterAssembly = Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging.Filter"));
                NLogger.LogManager.AddHiddenAssembly(filterAssembly);
            }
            catch (Exception)
            {
            }
            NLogger.LogManager.AddHiddenAssembly(typeof(Logger).GetTypeInfo().Assembly);

            using (var provider = new NLogLoggerProvider(options))
            {
                factory.AddProvider(provider);
            }
            return factory;
        }

        /// <summary>
        /// Apply NLog configuration from XML config.
        /// </summary>
        /// <param name="factory"><see cref="ILoggerFactory"/></param>
        /// <param name="fileName">NLog configuration file.</param>
        public static NLogger.Config.LoggingConfiguration ConfigureNLog(this ILoggerFactory factory, string fileName)
        {
            return ConfigureNLog(fileName);
        }

        /// <summary>
        /// Apply NLog configuration from XML config.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="configFileRelativePath">relative path to NLog configuration file.</param>
        public static NLogger.Config.LoggingConfiguration ConfigureNLog(this IHostingEnvironment env, string configFileRelativePath)
        {
            var fileName = Path.Combine(env.ContentRootPath, configFileRelativePath);
            return ConfigureNLog(fileName);
        }

        private static NLogger.Config.LoggingConfiguration ConfigureNLog(string fileName)
        {
            NLogger.LogManager.Configuration = new NLogger.Config.XmlLoggingConfiguration(fileName, true);
            return NLogger.LogManager.Configuration;
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        public static void Error(this ILogger logger, string message)
        {
            logger.LogError(0, message);
        }

        /// <summary>
        /// Errors the specified format.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void Error(this ILogger logger, string format, params object[] args)
        {
            logger.LogError(0, format, args);
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        /// <param name="error">The error.</param>
        public static void Error(this ILogger logger, string message, Exception error)
        {
            logger.LogError(0, error, message);
        }

        /// <summary>
        /// Errors the specified error.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="error">The error.</param>
        public static void Error(this ILogger logger, Exception error)
        {
            logger.LogError(0, error, error.Message);
        }

        /// <summary>
        /// Warn the specified message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        public static void Warn(this ILogger logger, string message)
        {
            logger.LogWarning(0, message);
        }

        /// <summary>
        /// Warn the specified format.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void Warn(this ILogger logger, string format, params object[] args)
        {
            logger.LogWarning(0, format, args);
        }

        /// <summary>
        /// Warn the specified message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Warn(this ILogger logger, string message, Exception exception)
        {
            logger.LogWarning(0, exception, message);
        }

        /// <summary>
        /// Warn the specified error.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="exception">The exception.</param>
        public static void Warn(this ILogger logger, Exception exception)
        {
            logger.LogWarning(0, exception, exception.Message);
        }
    }
}
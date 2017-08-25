#if NETSTANDARD2_0

using System;
using System.Reflection;
using Microsoft.Extensions.Logging;
using NLogger = NLog;

namespace Es.Extensions.Logging.NLog
{
    /// <summary>
    /// Extends <see cref="ILoggingBuilder"/> with NLog configuration methods.
    /// </summary>
    public static class LoggingBuilderExtensions
    {
        /// <summary>
        /// Add NLog to the logging pipeline.
        /// </summary>
        /// <param name="builder">The <see cref="T:Microsoft.Extensions.Logging.ILoggingBuilder" /> to add logging provider to.</param>
        /// <param name="logFactory"><see cref="NLogger.LogFactory"/></param>
        /// <param name="options"><see cref="NLogProviderOptions"/></param>
        /// <returns>The logger factory.</returns>
        public static ILoggingBuilder AddNLog(this ILoggingBuilder builder, NLogger.LogFactory logFactory, NLogProviderOptions options = null)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddProvider(new NLogLoggerProvider(logFactory, options));

            return builder;
        }

        /// <summary>
        /// Enable NLog as logging provider in ASP.NET Core.
        /// </summary>
        /// <param name="builder">The <see cref="T:Microsoft.Extensions.Logging.ILoggingBuilder" /> to add logging provider to.</param>
        /// <param name="options"><see cref="NLogProviderOptions"/></param>
        /// <returns></returns>
        public static ILoggingBuilder AddNLog(this ILoggingBuilder builder, NLogProviderOptions options = null)
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
                builder.AddProvider(provider);
            }
            return builder;
        }

        /// <summary>
        /// Apply NLog configuration from XML config.
        /// </summary>
        /// <param name="builder">The <see cref="T:Microsoft.Extensions.Logging.ILoggingBuilder" /> to add logging provider to.</param>
        /// <param name="fileName">NLog configuration file.</param>
        public static NLogger.Config.LoggingConfiguration ConfigureNLog(this ILoggingBuilder builder, string fileName)
        {
            return ConfigureNLog(fileName);
        }

        private static NLogger.Config.LoggingConfiguration ConfigureNLog(string fileName)
        {
            NLogger.LogManager.Configuration = new NLogger.Config.XmlLoggingConfiguration(fileName, true);
            return NLogger.LogManager.Configuration;
        }
    }
}
#endif
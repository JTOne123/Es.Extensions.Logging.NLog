using System;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logFactory = new LoggerFactory();

            Fileload(logFactory);

            Console.WriteLine("done.");
            Console.Read();
        }

        private static void Fileload(LoggerFactory logFactory)
        {
            var filename = AppContext.BaseDirectory + "/nlog.xml";

            logFactory.ConfigureNLog(filename);

            Console.WriteLine("Targets:" + NLog.LogManager.Configuration.AllTargets.Count);

            logFactory.AddNLog();

            var log = logFactory.CreateLogger("ConsoleDemo");

            log.LogTrace("Trace....");
            log.LogError("Verbose...");
            log.LogInformation("Information....");
            log.LogError("Error...");
            log.LogWarning("Warning...");
            log.LogCritical("Fatal...");

            var exception = new InvalidOperationException("Invalid value");

            log.LogError(0, exception, exception.Message);
        }

        private static void Sample(LoggerFactory logFactory)
        {
            Console.WriteLine("=============== sample ==================");

            LoggingConfiguration config = new LoggingConfiguration();
            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);
            LoggingRule rule1 = new LoggingRule("*", NLog.LogLevel.Trace, consoleTarget);
            config.LoggingRules.Add(rule1);
            LogFactory factory = new LogFactory(config);

            logFactory.AddNLog(factory);

            var log = logFactory.CreateLogger("ConsoleDemo");

            log.LogTrace("Trace....");
            log.LogError("Verbose...");
            log.LogInformation("Information....");
            log.LogError("Error...");
            log.LogWarning("Warning...");
            log.LogCritical("Fatal...");

            var exception = new InvalidOperationException("Invalid value");

            log.LogError(0, exception, exception.Message);
        }
    }
}
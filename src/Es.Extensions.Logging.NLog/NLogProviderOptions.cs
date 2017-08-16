using Microsoft.Extensions.Logging;
using NLog;

namespace Es.Extensions.Logging.NLog
{
    /// <summary>
    /// NLogProviderOptions
    /// </summary>
    public class NLogProviderOptions
    {
        /// <summary>
        /// Separator between for EventId.Id and EventId.Name. Default to .
        /// </summary>
        public string EventIdSeparator { get; set; }

        /// <summary>
        /// Skip allocation of <see cref="LogEventInfo.Properties" />-dictionary when <see cref="EventId"/>
        /// </summary>
        public bool IgnoreEmptyEventId { get; set; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public NLogProviderOptions()
        {
            EventIdSeparator = ".";
        }

        /// <summary>
        /// Default options
        /// </summary>
        internal static NLogProviderOptions Default = new NLogProviderOptions();
    }
}
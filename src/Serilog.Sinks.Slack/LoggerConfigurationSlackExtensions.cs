namespace Serilog
{
    using System;
    using Serilog.Configuration;
    using Serilog.Events;
    using Serilog.Formatting.Display;
    using Serilog.Sinks.Slack;

    /// <summary>
    /// Adds the WriteTo.Slack() extension method to <see cref="LoggerConfiguration"/>.
    /// </summary>
    public static class LoggerConfigurationSlackExtensions
    {
        private const string DefaultOutputTemplate ="*{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}]* {Message}";

        public static LoggerConfiguration Slack(
            this LoggerSinkConfiguration sinkConfiguration,
            string webHookUrl,
            string channel,
            string userName,
            string iconUrl=null,
            string iconEmoji=null,
            string outputTemplate = DefaultOutputTemplate,
            IFormatProvider formatProvider = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
        {
            if (sinkConfiguration == null) throw new ArgumentNullException("sinkConfiguration");

            var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
            var sink = new SlackSink(webHookUrl, channel, userName,iconUrl, iconEmoji, formatter);
            return sinkConfiguration.Sink(sink, restrictedToMinimumLevel);

        }

    }
}


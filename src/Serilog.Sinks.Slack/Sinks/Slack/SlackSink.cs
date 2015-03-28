namespace Serilog.Sinks.Slack
{
    using System;
    using System.IO;
    using System.Linq;
    using Client;
    using Core;
    using Debugging;
    using Events;
    using Formatting;
    using Parsing;

    internal class SlackSink : ILogEventSink, IDisposable
    {
        private readonly string channel;
        private readonly string userName;
        private readonly string iconUrl;
        private readonly string iconEmoji;
        private readonly ITextFormatter textFormatter;
        private readonly object syncRoot = new object();
        private SlackClientWebhooks client;

        public SlackSink(string webHookUrl, string channel, string userName, string iconUrl, string iconEmoji, ITextFormatter textFormatter)
        {
            if (textFormatter == null) throw new ArgumentNullException("textFormatter");

            this.channel = channel;
            this.userName = userName;
            this.iconUrl = iconUrl;
            this.iconEmoji = iconEmoji;
            this.textFormatter = textFormatter;
            client = new SlackClientWebhooks(webHookUrl);
        }

        /// <summary>
        /// Emit the provided log event to the sink.
        /// </summary>
        /// <param name="logEvent">The log event to write.</param>
        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null) throw new ArgumentNullException("logEvent");

            try
            {
                lock (syncRoot)
                {
                    using (var stringWriter = new StringWriter())
                    {
                        textFormatter.Format(logEvent, stringWriter);
                        
                        var p = new Arguments
                        {
                            Channel = channel,
                            Username = userName,
                            Text = stringWriter.ToString(),
                            IconUrl = iconUrl,
                            IconEmoji = iconEmoji
                        };

                        var a = new Attachment { Color = GetSlackColorFromLogEventLevel(logEvent.Level)};

                        var td = logEvent.MessageTemplate.Tokens.OfType<PropertyToken>().ToDictionary(pt => pt.PropertyName);


                        if (logEvent.Exception !=null)
                        {
                            var ae = new Attachment()
                            {
                                Pretext = "Exception:",
                                Color = GetSlackColorFromLogEventLevel(logEvent.Level),
                                Text = logEvent.Exception.ToString()
                            };
                            p.Attachments.Add(ae);
                 
                        }


                        foreach (var af in from pr in logEvent.Properties.OrderBy(o=>o.Key) where !td.ContainsKey(pr.Key) select new AttachmentFields
                        {
                            Title = pr.Key,
                            Value = pr.Value.ToString().Replace("\"", ""),
                            Short = false
                        })
                        {
                            a.Fields.Add(af);
                        }

                        p.Attachments.Add(a);
                        client.PostMessage(p);
                    }
                }
            }
            catch (Exception e)
            {
                SelfLog.WriteLine("Failed to send slack message. {0}", e);
            }
        }

        public void Dispose()
        {
            client = null;
        }

        private string GetSlackColorFromLogEventLevel(LogEventLevel level)
        {
            switch (level)
            {
                case LogEventLevel.Warning:
                    return "warning";

                case LogEventLevel.Error:
                case LogEventLevel.Fatal:
                    return "danger";

                case LogEventLevel.Information:
                    return "#2a80b9";

                default:
                    return "#cccccc";
            }
        }
    }
}
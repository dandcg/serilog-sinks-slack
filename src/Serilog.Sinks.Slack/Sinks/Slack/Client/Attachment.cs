namespace Serilog.Sinks.Slack.Client
{
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;

    public class Attachment
    {
        public Attachment()
        {
            Fields = new ObservableCollection<AttachmentFields>();
        }
        [JsonProperty("fallback")]
        public string Fallback { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("pretext")]
        public string Pretext { get; set; }
        [JsonProperty("color")]
        public string Color { get; set; }
        [JsonProperty("fields")]
        public ObservableCollection<AttachmentFields> Fields { get; set; }
    }
}
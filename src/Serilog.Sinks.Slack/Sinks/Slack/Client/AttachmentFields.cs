namespace Serilog.Sinks.Slack.Client
{
    using Newtonsoft.Json;

    public class AttachmentFields
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("short")]
        public bool Short { get; set; }
    }
}
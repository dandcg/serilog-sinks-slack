namespace Serilog.Sinks.Slack.Client
{
    using Newtonsoft.Json;

    public class Response
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }
        [JsonProperty("channel")]
        public string Channel { get; set; }
        [JsonProperty("ts")]
        public string TimeStamp { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
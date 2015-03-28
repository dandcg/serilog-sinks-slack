namespace Serilog.Sinks.Slack.Client
{
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;

    public class Arguments
    {
        public Arguments()
        {
            Attachments = new ObservableCollection<Attachment>();
            Parse = "full";
        }
        [JsonProperty("channel")]
        public string Channel { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("parse")]
        public string Parse { get; set; }
        [JsonProperty("link_names")]
        public string LinkNames { get; set; }
        [JsonProperty("unfurl_links")]
        public string UnfurlLinks { get; set; }
        [JsonProperty("unfurl_media")]
        public string UnfurlMedia { get; set; }
        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }
        [JsonProperty("icon_emoji")]
        public string IconEmoji { get; set; }
        [JsonProperty("attachments")]
        public ObservableCollection<Attachment> Attachments { get; set; }
    }
}
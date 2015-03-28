namespace Serilog.Sinks.Slack.Client
{
    using System;
    using System.Collections.Specialized;
    using System.Net;
    using System.Text;
    using Newtonsoft.Json;

    public class SlackClientWebhooks
    {
        private readonly Uri uri;
        private readonly Encoding encoding = new UTF8Encoding();
        public SlackClientWebhooks(string urlWithAccessToken)
        {
            uri = new Uri(urlWithAccessToken);
        }
        //Post a message using simple strings
        public void PostMessage(string text, string username = null, string channel = null)
        {
            var args = new Arguments()
            {
                Channel = channel,
                Username = username,
                Text = text
            };
            PostMessage(args);
        }
        //Post a message using a args object
        public void PostMessage(Arguments args)
        {
            string argsJson = JsonConvert.SerializeObject(args);
            using (var client = new WebClient())
            {
                var data = new NameValueCollection();
                data["payload"] = argsJson;
                var response = client.UploadValues(uri, "POST", data);
                //The response text is usually "ok"
                string responseText = encoding.GetString(response);
            }
        }
    }

    //This classes serializes into the Json payload required by Slack Incoming WebHooks
}


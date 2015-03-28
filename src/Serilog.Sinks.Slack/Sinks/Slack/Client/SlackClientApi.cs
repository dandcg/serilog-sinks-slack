namespace Serilog.Sinks.Slack.Client
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using Newtonsoft.Json;

    public class SlackClientApi
    {
        private readonly Uri uri;
        private readonly Encoding encoding = new UTF8Encoding();
        public SlackClientApi()
        {
            uri = new Uri("https://slack.com/api/chat.postMessage");
        }
        //Post a message using simple strings
        public void PostMessage(string token, string text, string username = null, string channel = null)
        {
            Arguments args = new Arguments()
            {
                Token = token,
                Channel = channel,
                Username = username,
                Text = text
            };

            PostMessage(args);
        }

        private string ToQueryString(Object p)
        {
            var properties = new List<string>();

            foreach (PropertyInfo propertyInfo in p.GetType().GetProperties())
            {
                if (propertyInfo.CanRead)
                {
                    var jsonProperty = propertyInfo.GetCustomAttributes(true).Where(x => x is JsonPropertyAttribute).Select(x => ((JsonPropertyAttribute)x).PropertyName).FirstOrDefault();
                    if (propertyInfo.PropertyType == typeof(ObservableCollection<Attachment>))
                    {
                        if (propertyInfo.GetValue(p, null) != null)
                        {
                            properties.Add(string.Format("{0}={1}", jsonProperty ?? propertyInfo.Name, WebUtility.UrlEncode(JsonConvert.SerializeObject(propertyInfo.GetValue(p, null)))));
                        }
                    }
                    else
                    {
                        if (propertyInfo.GetValue(p, null) != null)
                        {
                            properties.Add(string.Format("{0}={1}", jsonProperty ?? propertyInfo.Name, WebUtility.UrlEncode(propertyInfo.GetValue(p, null).ToString())));
                        }
                    }

                }
            }

            return string.Join("&", properties.ToArray());
        }

        public NameValueCollection ToQueryNVC(Object p)
        {

            NameValueCollection nvc = new NameValueCollection();

            foreach (PropertyInfo propertyInfo in p.GetType().GetProperties())
            {
                if (propertyInfo.CanRead)
                {
                    string jsonProperty = propertyInfo.GetCustomAttributes(true).Where(x => x is JsonPropertyAttribute).Select(x => ((JsonPropertyAttribute)x).PropertyName).FirstOrDefault();
                    if (propertyInfo.PropertyType == typeof(ObservableCollection<Attachment>))
                    {
                        if (propertyInfo.GetValue(p, null) != null)
                        {
                            nvc[jsonProperty ?? propertyInfo.Name] = JsonConvert.SerializeObject(propertyInfo.GetValue(p, null));
                        }
                    }
                    else
                    {
                        if (propertyInfo.GetValue(p, null) != null)
                        {
                            nvc[jsonProperty ?? propertyInfo.Name] = propertyInfo.GetValue(p, null).ToString();
                        }
                    }

                }
            }

            return nvc;

        }

        //Post a message using args object
        public Response PostMessage(Arguments args)
        {
            //string payloadJson = JsonConvert.SerializeObject(payload);
            using (WebClient client = new WebClient())
            {
                var data = ToQueryNVC(args);
                var response = client.UploadValues(uri, "POST", data);

                var responseText = encoding.GetString(response);

                return JsonConvert.DeserializeObject<Response>(responseText);
            }
        }

    }
}
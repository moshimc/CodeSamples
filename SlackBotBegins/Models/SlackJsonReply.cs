using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace SlackBotBegins.Models
{
    public class SlackJsonReply
    {
        [JsonProperty("channel")]
        public string channel { get; set; }
        [JsonProperty("as_user")]
        public bool as_user = true;
        [JsonProperty("text")]
        public string text { get; set; }
        [JsonProperty("attachments")]
        public List<Dictionary<string,string>> attachments { get; set; }

    }
}

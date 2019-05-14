using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlackBotBegins
{
    public class ChallengeResult
    {
        [JsonIgnore]
        [JsonProperty("token")]
        public string token { get; set; }
        [JsonProperty("challenge")]
        public string challenge { get; set; }
        [JsonIgnore]
        [JsonProperty("type")]
        public string type { get; set; }
    }
}

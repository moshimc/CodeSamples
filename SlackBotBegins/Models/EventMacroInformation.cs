using Newtonsoft.Json;

namespace SlackBotBegins
{
    public class EventMacroInformation

    {
        public string token { get; set; }
        public string team_id { get; set; }
        public string api_app_id { get; set; }
        [JsonProperty("event")]
        public EventInformation eventValues { get; set; }
        public string type { get; set; }
        public string event_id { get; set; }
        public string[] authed_teams { get; set; }   
        public string event_time { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

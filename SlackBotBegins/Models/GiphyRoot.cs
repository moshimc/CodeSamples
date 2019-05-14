using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlackBotBegins.Models
{
    public class GiphyRoot
    {
        [JsonProperty("data")]
        public List<GiphyMacroInformation> returnedGiphy { get; set; }

        public GiphyRoot()
        {
            returnedGiphy = new List<GiphyMacroInformation>();
        }
    }
}

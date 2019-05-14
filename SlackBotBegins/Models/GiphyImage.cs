using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlackBotBegins.Models
{
    public class GiphyImage
    {
        [JsonProperty("fixed_height_still")]
        public Dictionary<string,string> fixed_height_still_url { get; set; }
        [JsonProperty("original_still")]
        public Dictionary<string, string> original_still_url { get; set; }
        [JsonProperty("fixed_width")]
        public Dictionary<string, string> fixed_width_url { get; set; }
        [JsonProperty("fixed_height_small_still")]
        public Dictionary<string, string> fixed_height_small_still_url { get; set; }
        [JsonProperty("fixed_height_downsampled")]
        public Dictionary<string, string> fixed_height_downsampled_url { get; set; }
        [JsonProperty("preview")]
        public Dictionary<string, string> preview_url { get; set; }
        [JsonProperty("fixed_height_small")]
        public Dictionary<string, string> fixed_height_small_url { get; set; }
        [JsonProperty("downsized_still")]
        public Dictionary<string, string> downsized_still_url { get; set; }
        [JsonProperty("downsized")]
        public Dictionary<string, string> downsized_url { get; set; }
        [JsonProperty("downsized_large")]
        public Dictionary<string, string> downsized_large_url { get; set; }
        [JsonProperty("fixed_width_small_still")]
        public Dictionary<string, string> fixed_width_small_still_url { get; set; }
        [JsonProperty("preview_webp")]
        public Dictionary<string, string> preview_webp_url { get; set; }
        [JsonProperty("fixed_width_still")]
        public Dictionary<string, string> fixed_width_still_url { get; set; }
        [JsonProperty("fixed_width_small")]
        public Dictionary<string, string> fixed_width_small_url { get; set; }
        [JsonProperty("downsized_small")]
        public Dictionary<string, string> downsized_small_url { get; set; }
        [JsonProperty("fixed_width_downsampled")]
        public Dictionary<string, string> fixed_width_downsampled_url { get; set; }
        [JsonProperty("downsized_medium")]
        public Dictionary<string, string> downsized_medium_url { get; set; }
        [JsonProperty("original")]
        public Dictionary<string, string> original_url { get; set; }
        [JsonProperty("fixed_height")]
        public Dictionary<string, string> fixed_height_url { get; set; }
        [JsonProperty("looping")]
        public Dictionary<string, string> looping_url { get; set; }
        [JsonProperty("original_mp4")]
        public Dictionary<string, string> original_mp4_url { get; set; }
        [JsonProperty("preview_gif")]
        public Dictionary<string, string> preview_gif_url { get; set; }
        [JsonProperty("480w_still")]
        public Dictionary<string, string> still_480w_url { get; set; }
}
}

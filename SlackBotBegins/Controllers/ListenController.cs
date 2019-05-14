using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Google.Cloud.Translation.V2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SlackAPI;
using SlackBotBegins.Models;

namespace SlackBotBegins.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListenController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "hey", "me" };
            //return Ok(payload);          
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(EventMacroInformation eventMacroInformation)
        {
            // Only accept messages from users (not from bots)
            if (eventMacroInformation.eventValues.user != null)
            {
                // Remove the '@...' strings so bot doesn't call itself when submitting response
                string giphyText = ExtractGiphySearch(eventMacroInformation);

                // Translate our text search into either Japanese or ENglish and send to giphy
                Console.OutputEncoding = System.Text.Encoding.Unicode;
                TranslationClient googleClient = TranslationClient.Create();

                // Translating from English to JPN or vice versa?
                var detection = googleClient.DetectLanguage(text: giphyText);
                var languageCodeToTranslateTo = "";
                // Translating from Japanese to English
                if (detection.Language.Equals("ja"))
                {
                    languageCodeToTranslateTo = "en";
                }
                // Translating from English to Japanese
                else if (detection.Language.Equals("en"))
                {
                    languageCodeToTranslateTo = "ja";
                }
                var googleResponse = googleClient.TranslateText(giphyText, languageCodeToTranslateTo);

                // Now that we have our translation, search giphy
                GiphySearch giphySearch = new GiphySearch("http://api.giphy.com/v1/gifs/", googleResponse.TranslatedText, System.Environment.GetEnvironmentVariable("GIPHY_API_KEY"));
                string giphyResult = giphySearch.RetrieveGiphy();

                Dictionary<string, string> giphyInformation = new Dictionary<string, string>();
                giphyInformation.Add("text", languageCodeToTranslateTo + ": " + googleResponse.TranslatedText);
                giphyInformation.Add("image_url", giphyResult);

                // Create our JSON reply to send back to slack
                var reply = new SlackJsonReply();
                reply.channel = eventMacroInformation.eventValues.channel;
                reply.as_user = true;
                reply.text = giphyText;
                reply.attachments = new List<Dictionary<string, string>>();
                reply.attachments.Add(giphyInformation);

                var replyPayload = await Task.Run(() => JsonConvert.SerializeObject(reply));
                //var replyPayload = JsonConvert.SerializeObject(reply);
                var replyContent = new StringContent(replyPayload, System.Text.Encoding.UTF8, "application/json");

                Console.WriteLine(replyContent);

                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", System.Environment.GetEnvironmentVariable("SLACK_BOT_TOKEN"));
                var response = await client.PostAsync("https://slack.com/api/chat.postMessage", replyContent);
            }
            return Ok();
        }

        private static string ExtractGiphySearch(EventMacroInformation eventMacroInformation)
        {

            // Removed @[botname] from the text value we push to giphy
            string[] resultWords = eventMacroInformation.eventValues.text.Split(' ');
            string giphyText = "";
            foreach (var word in resultWords)
            {
                if (!word.Contains("@")) giphyText += word + " ";
            }

            return giphyText;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
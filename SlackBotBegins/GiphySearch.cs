using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SlackBotBegins.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SlackBotBegins
{
    public class GiphySearch
    {
        private readonly string _url;
        private readonly string _urlParameters;
        private readonly string _apiKey;

        public GiphySearch(string url, string urlParameters, string apiKey)
        {
            _url = url;
            _urlParameters = urlParameters;
            _apiKey = apiKey;
        }

        public string RetrieveGiphy()
        {
            string valueToReturn = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_url);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync("search?q=" + _urlParameters + "&api_key=" + _apiKey + "&limit=1").Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var dataObjects = response.Content.ReadAsAsync<GiphyRoot>().Result;  
                foreach (var d in dataObjects.returnedGiphy)
                {
                Console.WriteLine("{0}", d.images.original_url);
                }
                dataObjects.returnedGiphy.ElementAt(0).images.original_url.TryGetValue("url", out valueToReturn);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            //Make any other calls using HttpClient here.
            
            //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            client.Dispose();

            return valueToReturn;
        }
    }
}

using ApiClients.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiClients.Implementations
{
    public class RandomQuotesCaller:IRandomQuotesCaller
    {
        public async Task<Quote> GetQuotes(string lang="en")
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://quotes15.p.rapidapi.com/quotes/random/"),
                Headers =
                {
                    { "x-rapidapi-host", "quotes15.p.rapidapi.com" },
                    { "x-rapidapi-key", "3893ac1711msh7a6f3bdb3992da8p1b65b6jsn3d3f9335409f" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                JObject body =JObject.Parse( await response.Content.ReadAsStringAsync());
                var res = new Quote { Content=(string)body["content"],Name=(string)body["originator"]["name"]};
                return res;
            }
        }
    }
}

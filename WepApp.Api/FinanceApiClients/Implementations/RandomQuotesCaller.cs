using ApiClients.Configs;
using ApiClients.Models;
using Microsoft.Extensions.Options;
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
        CallerConfig<RandomQuotesCaller> _config;
        public RandomQuotesCaller(IOptions<CallerConfig<RandomQuotesCaller>> config)
        {
            this._config = config.Value;
        }
        public async Task<Quote> GetQuotes(string lang="en")
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://"+_config.Host+"/quotes/random/"),
                Headers =
                {
                    { "x-rapidapi-host", _config.Host },
                    { "x-rapidapi-key", _config.Key },
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

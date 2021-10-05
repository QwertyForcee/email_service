using ApiClients.Configs;
using ApiClients.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiClients
{
    public class CoinrankingCaller:ICoinrankingCaller
    {
        CallerConfig<CoinrankingCaller> _config;
        public CoinrankingCaller(IOptions<CallerConfig<CoinrankingCaller>> config)
        {
            this._config = config.Value;
        }
        public async Task<Coin> GetCoin(int id)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://"+_config.Host+ "/coin/"+id),
                Headers =
                {
                    { "x-rapidapi-host", _config.Host },
                    { "x-rapidapi-key", _config.Key },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                JObject coinBody = JObject.Parse(await response.Content.ReadAsStringAsync());
                var realcoin =coinBody["data"]["coin"].ToObject<Coin>(); 

                return realcoin;
            }
        }

        public async Task<List<Coin>> GetCoins()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://"+_config.Host+"/coins"),
                Headers =
                {
                    { "x-rapidapi-host", _config.Host },
                    { "x-rapidapi-key", _config.Key },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                JObject bodyJO = JObject.Parse(await response.Content.ReadAsStringAsync());
                JArray coinsBody = JArray.Parse(bodyJO["data"]["coins"].ToString());
                var coinList = new List<Coin>();
                foreach (JObject jobj in coinsBody)
                {
                    coinList.Add(jobj.ToObject<Coin>());
                }
                return coinList;
            }

        }
    }
}

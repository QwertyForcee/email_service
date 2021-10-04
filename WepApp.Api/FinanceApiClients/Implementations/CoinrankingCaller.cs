using ApiClients.Models;
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
        public async Task<Coin> GetCoin(int id)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://coinranking1.p.rapidapi.com/coin/"+id),
                Headers =
                {
                    { "x-rapidapi-host", "coinranking1.p.rapidapi.com" },
                    { "x-rapidapi-key", "3893ac1711msh7a6f3bdb3992da8p1b65b6jsn3d3f9335409f" },
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
                RequestUri = new Uri("https://coinranking1.p.rapidapi.com/coins"),
                Headers =
                {
                    { "x-rapidapi-host", "coinranking1.p.rapidapi.com" },
                    { "x-rapidapi-key", "3893ac1711msh7a6f3bdb3992da8p1b65b6jsn3d3f9335409f" },
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

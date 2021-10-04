using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiClients
{
    public class CurrencyExchangeCaller:ICurrencyExchangeCaller
    {
        public HttpClient client = new HttpClient();

        public async Task<List<string>> GetCurrenciesListAsync()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://currency-exchange.p.rapidapi.com/listquotes"),
                Headers =
                {
                    { "x-rapidapi-host", "currency-exchange.p.rapidapi.com" },
                    { "x-rapidapi-key", "3893ac1711msh7a6f3bdb3992da8p1b65b6jsn3d3f9335409f" },
                },
            };
            using (var response = await this.client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var res = JsonConvert.DeserializeObject<List<string>>( await response.Content.ReadAsStringAsync());
                return res;
            }
        }
        public async Task<object> GetExchangeCurrencyAsync(string from, string to,int count=1)
        {
            string uri = $"https://currency-exchange.p.rapidapi.com/exchange?from="+from+"&to="+to+"&q="+count;
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri),
                Headers =
                {
                    { "x-rapidapi-host", "currency-exchange.p.rapidapi.com" },
                    { "x-rapidapi-key", "3893ac1711msh7a6f3bdb3992da8p1b65b6jsn3d3f9335409f" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                return new { From = from, To = to, Count = count, Value = await response.Content.ReadAsStringAsync() };
            }
        }
    }
}

using ApiClients.Configs;
using Microsoft.Extensions.Options;
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
        CallerConfig<CurrencyExchangeCaller> _config;
        public CurrencyExchangeCaller(IOptions<CallerConfig<CurrencyExchangeCaller>> config)
        {
            this._config = config.Value;
        }
        public HttpClient client = new HttpClient();

        public async Task<List<string>> GetCurrenciesListAsync()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://"+_config.Host+"/listquotes"),
                Headers =
                {
                    { "x-rapidapi-host", _config.Host },
                    { "x-rapidapi-key", _config.Key },
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
            string uri = $" https://" + _config.Host + "/exchange?from=" +from+"&to="+to+"&q="+count;
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri),
                Headers =
                {
                    { "x-rapidapi-host", _config.Host },
                    { "x-rapidapi-key", _config.Key },
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

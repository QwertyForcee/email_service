using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClients
{
    public interface ICurrencyExchangeCaller
    {
        Task<List<string>> GetCurrenciesListAsync();
        Task<object> GetExchangeCurrencyAsync(string from, string to,int count);
    }
}

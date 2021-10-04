using ApiClients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClients
{
    public interface ICoinrankingCaller
    {
        Task<Coin> GetCoin(int id);
        Task<List<Coin>> GetCoins();
    }
}

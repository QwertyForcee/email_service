using ApiClients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClients
{
    public interface IRandomQuotesCaller
    {
        Task<Quote> GetQuotes(string lang);
    }
}

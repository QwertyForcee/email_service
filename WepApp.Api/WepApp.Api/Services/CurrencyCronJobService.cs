using ApiClients;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WepApp.Api.DataAccess.Repositories;
using WepApp.Api.Entities;

namespace WepApp.Api.Services
{
    public class CurrencyCronJobService: CronJobService<CurrencyTask>
    {
        public CurrencyCronJobService(ILogger<CronJobService<CurrencyTask>> logger, IServiceProvider provider) : base(logger, provider)
        {
            this._logger = logger;
            this._serviceProvider = provider;
        }
        protected async override Task LoadCronJobs()
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                ICurrencyTaskRepository<CurrencyTask> currencyRep = scope.ServiceProvider.GetRequiredService<ICurrencyTaskRepository<CurrencyTask>>();
                foreach (var currencyTask in await currencyRep.GetAllAsync())
                {
                    this.cronJobs.AddSorted(currencyTask);
                }
            }
        }
        protected async override Task<object> DoExactWork(ICronJob job)
        {
            object data = null;
            var currencyJob = (job as CurrencyTask);
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var coinCaller = scope.ServiceProvider.GetRequiredService<ICurrencyExchangeCaller>();
                data = await coinCaller.GetExchangeCurrencyAsync(currencyJob.From, currencyJob.To, currencyJob.Count);
            }

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var rep = scope.ServiceProvider.GetRequiredService<ICurrencyTaskRepository<CurrencyTask>>();
                currencyJob.LastTime = DateTime.Now.ToString();
                await rep.UpdateAsync(currencyJob);
            }
            return data;
        }
    }
}

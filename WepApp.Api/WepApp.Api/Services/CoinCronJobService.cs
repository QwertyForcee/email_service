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
    public class CoinCronJobService:CronJobService<CoinTask>
    {
        public CoinCronJobService(ILogger<CronJobService<CoinTask>> logger, IServiceProvider provider):base (logger,provider)
        {
            this._logger = logger;
            this._serviceProvider = provider;
        }
        protected async override Task LoadCronJobs()
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                ICoinTaskRepository<CoinTask> coinRep = scope.ServiceProvider.GetRequiredService<ICoinTaskRepository<CoinTask>>();

                foreach (var coinTask in await coinRep.GetAllAsync())
                {
                    this.cronJobs.AddSorted(coinTask);
                }
            }
        }
        protected async override Task<object> DoExactWork(ICronJob job)
        {
            object data = null;
            var coinTask = job as CoinTask;
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var coinCaller = scope.ServiceProvider.GetRequiredService<ICoinrankingCaller>();
                data = await coinCaller.GetCoin(coinTask.CoinId);
            }

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var rep = scope.ServiceProvider.GetRequiredService<ICoinTaskRepository<CoinTask>>();
                coinTask.LastTime = DateTime.Now.ToString();
                await rep.UpdateAsync(coinTask);
            }
            return data;
        }
    }
}

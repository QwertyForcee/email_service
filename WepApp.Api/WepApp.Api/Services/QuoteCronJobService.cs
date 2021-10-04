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
    public class QuoteCronJobService:CronJobService<QuoteTask>
    {
        public QuoteCronJobService(ILogger<CronJobService<QuoteTask>> logger, IServiceProvider provider) : base(logger, provider)
        {
            this._logger = logger;
            this._serviceProvider = provider;
        }
        protected async override Task LoadCronJobs()
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                IQuoteTaskRepository<QuoteTask> quoteRep = scope.ServiceProvider.GetRequiredService<IQuoteTaskRepository<QuoteTask>>();
                foreach (var quoteTask in await quoteRep.GetAllAsync())
                {
                    this.cronJobs.AddSorted(quoteTask);
                }
            }
        }
        protected async override Task<object> DoExactWork(ICronJob job)
        {
            object data = null;
            var quoteTask = job as QuoteTask;
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var coinCaller = scope.ServiceProvider.GetRequiredService<IRandomQuotesCaller>();
                data = await coinCaller.GetQuotes(quoteTask.Lang);

            }

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var rep = scope.ServiceProvider.GetRequiredService<IQuoteTaskRepository<QuoteTask>>();
                quoteTask.LastTime = DateTime.Now.ToString();
                await rep.UpdateAsync(quoteTask);
            }
            return data;
        }
    }
}

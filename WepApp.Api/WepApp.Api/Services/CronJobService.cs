using ApiClients;
using Cronos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WepApp.Api.DataAccess.Repositories;
using WepApp.Api.EmailManager;
using WepApp.Api.Entities;
using WepApp.Api.Models;

namespace WepApp.Api.Services
{
    public abstract class CronJobService<T> : BackgroundService where T:ICronJob
    {
        protected IServiceProvider _serviceProvider;

        protected Mutex mutex = new Mutex();
        protected int currentCountOfThreads = 0;
        protected int maxThreadsCount=500;

        protected ILogger<CronJobService<T>> _logger;

        protected List<T> cronJobs=new List<T>();
        protected List<T> currentJobs = new List<T>();
        public CronJobService(ILogger<CronJobService<T>> logger,IServiceProvider provider)
        {
            this._logger = logger;
            this._serviceProvider = provider;
        }

        protected abstract Task<object> DoExactWork(ICronJob job);
        protected abstract Task LoadCronJobs();
        protected async Task ManageJob(int id, CancellationToken cancellationToken)
        {
            var job = currentJobs.Where(j => j.Id == id).FirstOrDefault();
            System.Timers.Timer timer;
            CronExpression cron = CronExpression.Parse(job.CronExpression);
            var next = cron.GetNextOccurrence(DateTimeOffset.Now, TimeZoneInfo.Local);
            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                timer = new System.Timers.Timer(delay.TotalMilliseconds);
                timer.Elapsed +=  async (sender, args) =>
                {
                    timer.Dispose();
                    timer = null;

                    //do job;
                    await DoWork(job.Id);
                    this.mutex.WaitOne();
                    this._logger.LogInformation("finish work:"+job.Name);
                    this.currentCountOfThreads--;
                    this.cronJobs.AddSorted(job);
                    this.currentJobs.Remove(job);
                    this.mutex.ReleaseMutex();
                };
                await Task.Run(()=>timer.Start());
            }
        }
        protected async Task DoWork(int id)
        {
            var job = this.currentJobs.Where(j => j.Id == id).FirstOrDefault();
            if (job != null)
            {
                object data = await DoExactWork(job);

                if (data != null)
                {
                    using (IServiceScope scope = _serviceProvider.CreateScope()) {
                        var csvManager = scope.ServiceProvider.GetRequiredService<ICsvManager>();
                        var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

                        var filename = csvManager.WriteCsv(data);
                        emailSender.Send(job, filename);
                    }
                }
                else
                {
                    this._logger.LogInformation("data=null");
                }
            }
        }

        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await LoadCronJobs();
            this.cronJobs.OrderByDescending(j => j.TimeToExec());
            _logger.LogInformation("sheduling job");

            while (!cancellationToken.IsCancellationRequested)
            {
                if (this.currentCountOfThreads < this.maxThreadsCount)
                {
                    if (this.cronJobs.Count > 0)
                    {
                        _logger.LogInformation("creating new job");
                        var job = this.cronJobs[0];
                        this.cronJobs.RemoveAt(0);
                        this.currentJobs.Add(job);
                        //var task = new Task();
                        await Task.Run(async () => await this.ManageJob(job.Id, cancellationToken));
                        this.mutex.WaitOne();
                        this.currentCountOfThreads++;
                        this.mutex.ReleaseMutex();
                    }
                }
            }
        }
        public void AddCronJob(T job)
        {
            this._logger.LogInformation("Job added. Name:" + job.Name+" id:"+job.Id);
            this.mutex.WaitOne();
            this.cronJobs.AddSorted(job);
            this.mutex.ReleaseMutex();
        }
        public void DeleteCronJob(int id)
        {
            this._logger.LogInformation("Deleting job with id:" + id);
            this.mutex.WaitOne();

            this.cronJobs = this.cronJobs.Where(j => j.Id != id).ToList();
            this.currentJobs = this.currentJobs.Where(j => j.Id != id).ToList();
            
            this.mutex.ReleaseMutex();
        }
        public void UpdateCronJob(T job)
        {
            this.mutex.WaitOne();
            var cr = this.cronJobs.Where(j => j.Id == job.Id).FirstOrDefault();
            if (cr != null)
            {
                cr = job;
            }
            else
            {
                cr = this.currentJobs.Where(j => j.Id == job.Id).FirstOrDefault();
                if (cr != null)
                {
                    cr = job;
                }
            }
            this.mutex.ReleaseMutex();
            if (cr == null) this.AddCronJob(job);
        }

    }
    public class CronJobComparerDescend : IComparer<ICronJob>
    {
        public CronJobComparerDescend()
        {

        }
        public int Compare(ICronJob x, ICronJob y)
        {
            var xtime = x.TimeToExec();
            var ytime = y.TimeToExec();
            if (xtime == ytime) return 0;

            return xtime > ytime ? -1 : 1;
        }
    }

    public static class CronListExt
    {
        public static void AddSorted<ICronJob>(this List<ICronJob> @this, ICronJob job)
        {
            IComparer<ICronJob> c = (IComparer<ICronJob>)(new CronJobComparerDescend());
            int index = @this.BinarySearch(job,c);
            if (index < 0) index = ~index;
            @this.Insert(index, job);
        }
    }
    public static class ServiceProviderExtensions
    {
        public static CronJobService GetHostedService<CronJobService>
            (this IServiceProvider serviceProvider) =>
            serviceProvider
                .GetServices<IHostedService>()
                .OfType<CronJobService>()
                .FirstOrDefault();
    }
}

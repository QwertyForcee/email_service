using ApiClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WepApp.Api.DataAccess.Repositories;
using WepApp.Api.Entities;
using WepApp.Api.Services;

namespace WepApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private int UserId => Int32.Parse(this.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
        private ICurrencyTaskRepository<CurrencyTask> taskRepository;
        private ICurrencyExchangeCaller currencyCaller;
        public CurrencyController(ICurrencyTaskRepository<CurrencyTask> taskRepository, ICurrencyExchangeCaller caller)
        {
            this.taskRepository = taskRepository;
            this.currencyCaller = caller;
        }

        [HttpGet("tasks")]
        public async Task<IActionResult> GetTasks()
        {
            return Ok(await this.taskRepository.GetAllAsync());
        }

        [HttpGet("tasks/user")]
        public async Task<IActionResult> GetUserTasks()
        {
            return Ok(await this.taskRepository.GetUserTasksAsync(this.UserId));
        }

        [HttpPost("tasks")]
        public async Task<IActionResult> PostCurrencyTask(CurrencyTask currencyTask)
        {
            currencyTask.UserId = this.UserId;

            var res = await this.taskRepository.AddAsync(currencyTask);
            currencyTask = (await this.taskRepository.GetUserTasksAsync(currencyTask.UserId))
                                                    .Where(c => c.Name == currencyTask.Name && c.Description == currencyTask.Description
                                                              && c.CronExpression == currencyTask.CronExpression)
                                                    .FirstOrDefault();

            var cronJobService = this.ControllerContext.HttpContext.RequestServices.GetHostedService<CurrencyCronJobService>();
            cronJobService.AddCronJob(currencyTask);

            return Ok(res);
        }

        [HttpPut("tasks")]
        public async Task<IActionResult> PutCurrencyTask(CurrencyTask currencyTask)
        {
            var res = await this.taskRepository.UpdateAsync(currencyTask);
            var cronJobService = this.ControllerContext.HttpContext.RequestServices.GetHostedService<CurrencyCronJobService>();
            cronJobService.UpdateCronJob(currencyTask);

            return Ok(res);
        }

        [HttpDelete("tasks/{id}")]
        public async Task<IActionResult> DeleteCurrencyTask(int id)
        {
            var res = await this.taskRepository.DeleteAsync(id);

            var cronJobService = this.ControllerContext.HttpContext.RequestServices.GetHostedService<CurrencyCronJobService>();
            cronJobService.DeleteCronJob(id);

            return Ok();
        }
        [HttpGet("tasks/{id}")]
        public async Task<IActionResult> GetCurrencyTask(int id)
        {
            return Ok(await this.taskRepository.GetAsync(id));
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            return Ok(await currencyCaller.GetCurrenciesListAsync());
        }
        [HttpGet("exchange")]
        public async Task<IActionResult> Get()
        {
            //var res = JsonSerializer.Deserialize<JsonElement> await caller.GetExchangeCurrency("USD", "RUB");
            return Ok(await currencyCaller.GetExchangeCurrencyAsync("USD", "RUB",1));
        }
    }
}

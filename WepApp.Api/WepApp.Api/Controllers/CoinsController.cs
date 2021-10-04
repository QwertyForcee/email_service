using ApiClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using WepApp.Api.DataAccess.Repositories;
using WepApp.Api.Entities;
using WepApp.Api.Services;

namespace WepApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CoinsController : ControllerBase
    {
        private int UserId => Int32.Parse(this.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
        private ICoinrankingCaller coinCaller;
        private ICoinTaskRepository<CoinTask> coinTaskRep;
        public CoinsController(ICoinrankingCaller caller, ICoinTaskRepository<CoinTask> coinTaskRep)
        {
            this.coinCaller = caller;
            this.coinTaskRep = coinTaskRep;
        }


        [HttpGet("tasks")]
        public async Task<IActionResult> GetTasks()
        {
            return Ok(await this.coinTaskRep.GetAllAsync());
        }
        [HttpGet("tasks/user")]
        public async Task<IActionResult> GetUserTasks()
        {
            return Ok(await this.coinTaskRep.GetUserTasksAsync(this.UserId));
        }

        [HttpPost("tasks")]
        public async Task<IActionResult> PostCoinTask(CoinTask coinTask)
        {
            coinTask.UserId = this.UserId;
            var res = await this.coinTaskRep.AddAsync(coinTask);
            coinTask = (await this.coinTaskRep.GetUserTasksAsync(coinTask.UserId))
                                                    .Where(c => c.Name == coinTask.Name && c.Description==coinTask.Description
                                                              && c.CronExpression==coinTask.CronExpression)
                                                    .FirstOrDefault();

            var cronJobService=  this.ControllerContext.HttpContext.RequestServices.GetHostedService<CoinCronJobService>();
            cronJobService.AddCronJob(coinTask);
            
            return Ok(res);
        }

        [HttpPut("tasks")]
        public async Task<IActionResult> PutCoinTask(CoinTask coinTask)
        {
            var res = await this.coinTaskRep.UpdateAsync(coinTask);
            var cronJobService = this.ControllerContext.HttpContext.RequestServices.GetHostedService<CoinCronJobService>();
            cronJobService.UpdateCronJob(coinTask);

            return Ok(res);
        }

        [HttpDelete("tasks/{id}")]
        public async Task<IActionResult> DeleteCoinTask(int id)
        {
            var res = await this.coinTaskRep.DeleteAsync(id);
            
            var cronJobService = this.ControllerContext.HttpContext.RequestServices.GetHostedService<CoinCronJobService>();
            cronJobService.DeleteCronJob(id);

            return Ok();
        }
        [HttpGet("tasks/{id}")]
        public async Task<IActionResult> GetCoinTask(int id)
        {
            return Ok(await this.coinTaskRep.GetAsync(id));
        }

        [HttpGet("coins/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var coin = await coinCaller.GetCoin(id);

            return Ok(coin);
        }

        [HttpGet("coins")]
        public async Task<IActionResult> GetCoins()
        {
            return Ok(await this.coinCaller.GetCoins());
        }
    }
}

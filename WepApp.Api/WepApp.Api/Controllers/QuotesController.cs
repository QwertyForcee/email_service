using ApiClients;
using ApiClients.Implementations;
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
    public class QuotesController : ControllerBase
    {
        private int UserId => Int32.Parse(this.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
        private IRandomQuotesCaller quotesCaller;
        private IQuoteTaskRepository<QuoteTask> taskRepository;
        public QuotesController(IRandomQuotesCaller quotesCaller, IQuoteTaskRepository<QuoteTask> taskRepository)
        {
            this.quotesCaller = quotesCaller;
            this.taskRepository = taskRepository;
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
        public async Task<IActionResult> PostQuoteTask(QuoteTask quoteTask)
        {
            quoteTask.UserId = this.UserId;
            var res = await this.taskRepository.AddAsync(quoteTask);
            quoteTask = (await this.taskRepository.GetUserTasksAsync(quoteTask.UserId))
                                                    .Where(c => c.Name == quoteTask.Name && c.Description == quoteTask.Description
                                                              && c.CronExpression == quoteTask.CronExpression)
                                                    .FirstOrDefault();

            var cronJobService = this.ControllerContext.HttpContext.RequestServices.GetHostedService<QuoteCronJobService>();
            cronJobService.AddCronJob(quoteTask);

            return Ok(res);
        }

        [HttpPut("tasks")]
        public async Task<IActionResult> PutQuoteTask(QuoteTask quoteTask)
        {
            var res = await this.taskRepository.UpdateAsync(quoteTask);
            var cronJobService = this.ControllerContext.HttpContext.RequestServices.GetHostedService<QuoteCronJobService>();
            cronJobService.UpdateCronJob(quoteTask);

            return Ok(res);
        }

        [HttpDelete("tasks/{id}")]
        public async Task<IActionResult> DeleteQuoteTask(int id)
        {
            var res = await this.taskRepository.DeleteAsync(id);

            var cronJobService = this.ControllerContext.HttpContext.RequestServices.GetHostedService<QuoteCronJobService>();
            cronJobService.DeleteCronJob(id);

            return Ok();
        }
        [HttpGet("tasks/{id}")]
        public async Task<IActionResult> GetQuoteTask(int id)
        {
            return Ok(await this.taskRepository.GetAsync(id));
        }


        [HttpGet("{lang}")]
        public async Task<IActionResult> Get(string lang="en")
        {
            return Ok(await this.quotesCaller.GetQuotes(lang));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Cards_Products_API.Services;
using Quartz;

namespace Cards_Products_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly RabbitMQService _rabbitMQ;

        public JobsController(
            ISchedulerFactory schedulerFactory,
            RabbitMQService rabbitMQ)  // ← Agregar parámetro
        {
            _schedulerFactory = schedulerFactory;
            _rabbitMQ = rabbitMQ;  // ← Inicializar
        }

        [HttpPost("GenerateUsers")]
        public async Task<IActionResult> RunGenerateUsersJob()
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKey = new JobKey("GenerateUsersJob");

            if (!await scheduler.CheckExists(jobKey))
                return NotFound("Job no registrado");

            var trigger = TriggerBuilder.Create()
                .ForJob(jobKey)
                .StartNow()
                .Build();

            await scheduler.ScheduleJob(trigger);

            return Ok("GenerateUsersJob disparado!");
        }

        [HttpPost("generateData")]
        public async Task<IActionResult> RunGenerateDataJob()
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKey = new JobKey("GenerateDataJob");

            if (!await scheduler.CheckExists(jobKey))
                return NotFound("Job no registrado");

            var trigger = TriggerBuilder.Create()
                .ForJob(jobKey)
                .StartNow()
                .Build();

            await scheduler.ScheduleJob(trigger);

            return Ok("GenerateDataJob disparado!");
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> RunPurchaseJob()
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKey = new JobKey("PurchaseJob");

            if (!await scheduler.CheckExists(jobKey))
                return NotFound("Job no registrado");

            var trigger = TriggerBuilder.Create()
                .ForJob(jobKey)
                .StartNow()
                .Build();

            await scheduler.ScheduleJob(trigger);

            return Ok("PurchaseJob disparado!");
        }
    }
}

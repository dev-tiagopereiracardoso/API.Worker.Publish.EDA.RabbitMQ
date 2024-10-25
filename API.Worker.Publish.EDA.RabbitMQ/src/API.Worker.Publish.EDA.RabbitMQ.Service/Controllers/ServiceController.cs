using API.Worker.Publish.EDA.RabbitMQ.Domain.Implementation.Interfaces;
using API.Worker.Publish.EDA.RabbitMQ.Models.Events;
using Microsoft.AspNetCore.Mvc;

namespace API.Worker.Publish.EDA.RabbitMQ.Service.Controllers
{
    [Route("v1/services")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "publish")]
    public class ServiceController : ControllerBase
    {
        private readonly ILogger<ServiceController> _logger;

        private readonly IProducerService _producerService;

        public ServiceController(
                ILogger<ServiceController> logger,
                IProducerService producerService
            )
        {
            _logger = logger;
            _producerService = producerService;
        }

        [HttpPost("send")]
        public IActionResult Post(RegisterUserEvent Json)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return StatusCode(_producerService.Publish(Json));
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message);

                return BadRequest();
            }
        }
    }
}
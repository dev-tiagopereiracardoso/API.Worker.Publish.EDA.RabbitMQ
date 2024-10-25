using API.Worker.Publish.EDA.RabbitMQ.Domain.Implementation.Interfaces;
using API.Worker.Publish.EDA.RabbitMQ.Models.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Net;

namespace API.Worker.Publish.EDA.RabbitMQ.Domain.Implementation.Services
{
    public class ProducerService : IProducerService
    {
        private readonly ILogger<ProducerService> _logger;

        private readonly IPublishEndpoint _publishEndpoint;

        public ProducerService(
                ILogger<ProducerService> logger,
                IPublishEndpoint publishEndpoint
            )
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public int Publish(RegisterUserEvent Json)
        {
            try
            {
                _publishEndpoint.Publish(Json);

                _logger.LogInformation("Sent the input parameters to the Queue.");
            }
            catch (Exception Ex)
            {
                _logger.LogError($"There was a failure sending the parameters to the QUEUE. {Ex.Message}");

                return (int)HttpStatusCode.BadRequest;
            }

            return (int)HttpStatusCode.OK;
        }
    }
}
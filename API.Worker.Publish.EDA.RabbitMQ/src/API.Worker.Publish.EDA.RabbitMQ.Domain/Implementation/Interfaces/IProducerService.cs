using API.Worker.Publish.EDA.RabbitMQ.Models.Events;

namespace API.Worker.Publish.EDA.RabbitMQ.Domain.Implementation.Interfaces
{
    public interface IProducerService
    {
        int Publish(RegisterUserEvent Json);
    }
}
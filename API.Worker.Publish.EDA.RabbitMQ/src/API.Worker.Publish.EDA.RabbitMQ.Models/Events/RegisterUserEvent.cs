namespace API.Worker.Publish.EDA.RabbitMQ.Models.Events
{
    public class RegisterUserEvent
    {
        public string Name { set; get; }

        public string DocumentNumber { set; get; }
    }
}
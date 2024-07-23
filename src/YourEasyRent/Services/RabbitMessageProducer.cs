using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Telegram.Bot.Types;

namespace YourEasyRent.Services
{
    public class RabbitMessageProducer : IRabbitMessageProducer
    {
        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory 
            {
                HostName = "localhost",
                UserName = "rmq",
                Password = "pass"
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("creating_subscriber",
                     durable: true,
                     exclusive: true);
            var JsonString = JsonSerializer.Serialize(message);
            var body  = Encoding.UTF8.GetBytes(JsonString);
            channel.BasicPublish(exchange: "", "creating_subscriber", body: body);
        }
    }
}

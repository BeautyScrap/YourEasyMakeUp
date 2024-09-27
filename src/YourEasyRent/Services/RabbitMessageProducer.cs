using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Telegram.Bot.Types;

namespace YourEasyRent.Services
{
    public class RabbitMessageProducer : IRabbitMessageProducer
    {
        public void SendMessagAboutSubscriber<T>(T message)
        {
            var factory = new ConnectionFactory 
            {
                HostName = "localhost",
                UserName = "rmq",
                Password = "pass",
                VirtualHost = "/"
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("creating_subscriber",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            var JsonString = JsonSerializer.Serialize(message);
            var body  = Encoding.UTF8.GetBytes(JsonString);
            channel.BasicPublish(exchange: "", routingKey: "creating_subscriber", body: body);
        }

        public void ConsumingSubscriberMessag<T>(T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "rmq",
                Password = "pass",
                VirtualHost = "/"
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("Search_product_For_Subscriber",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" The message has been received {message}");
            };
            channel.BasicConsume(queue:"Search_product_For_Subscriber", autoAck: true, consumer: consumer);
        }

    }
}

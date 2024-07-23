using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client.Events;

namespace SubscriberAPI.Application
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
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
                {
                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" The message has been received {message}");
                };
            channel.BasicConsume(queue: "creating_subscriber",autoAck: true, consumer: consumer);



            //var JsonString = JsonSerializer.Serialize(message);
            //var body = Encoding.UTF8.GetBytes(JsonString);
            //channel.BasicPublish("", "creating_subscriber", body: body);
        }
    }
}

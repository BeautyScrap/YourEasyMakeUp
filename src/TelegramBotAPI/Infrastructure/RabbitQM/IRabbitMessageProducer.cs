namespace TelegramBotAPI.Infrastructure.RabbitQM
{
    public interface IRabbitMessageProducer
    {
        public void SendMessagAboutSubscriber<T>(T message);
        public void ConsumingSubscriberMessage<T>(T message);
    }
}

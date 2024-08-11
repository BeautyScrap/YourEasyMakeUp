namespace SubscriberAPI.Application.RabbitQM
{
    public interface IRabbitMessageProducer
    {
        public void ConsumingMessage<T>(T message);
    }
}

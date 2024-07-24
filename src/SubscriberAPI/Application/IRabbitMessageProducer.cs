namespace SubscriberAPI.Application
{
    public interface IRabbitMessageProducer
    {
        public void ConsumingMessage<T>(T message);
    }
}

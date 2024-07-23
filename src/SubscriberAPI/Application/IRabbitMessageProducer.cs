namespace SubscriberAPI.Application
{
    public interface IRabbitMessageProducer
    {
        public void SendMessage<T>(T message);
    }
}

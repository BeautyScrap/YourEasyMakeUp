namespace YourEasyRent.Services
{
    public interface IRabbitMessageProducer
    {
        public void SendMessagAboutSubscriber<T>(T message);
        public void ConsumingSubscriberMessag<T>(T message);
    }
}

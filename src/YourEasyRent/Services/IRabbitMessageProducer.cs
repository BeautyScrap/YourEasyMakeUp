namespace YourEasyRent.Services
{
    public interface IRabbitMessageProducer
    {
        public void SendMessagAboutSubscriber<T>(T message);
    }
}

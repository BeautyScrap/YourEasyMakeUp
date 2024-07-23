namespace YourEasyRent.Services
{
    public interface IRabbitMessageProducer
    {
        public void SendMessage<T>(T message);
    }
}

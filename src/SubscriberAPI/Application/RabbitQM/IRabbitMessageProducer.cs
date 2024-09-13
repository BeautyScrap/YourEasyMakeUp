namespace SubscriberAPI.Application.RabbitQM
{
    public interface IRabbitMessageProducer
    {
        public void ConsumingSubscriberMessag<T>(T message);

        public void SendProductSearchMessage<T>(T message);

    }
}

namespace ProductAPI.Application.RabbitMQ
{
    public interface IRabbitMessageProducer
    {
        public void ConsumingSubscriberMessag<T>(T message);

        public void SendProductSearchMessage<T>(T message);

    }
}

namespace SubscriberAPI.Application.RabbitQM
{
    public interface ISubscriberRabbitMessageProducer
    {
        public void ConsumingSubscriberMessag<T>(T message);

        public void SendProductSearchMessage<T>(T message);

    }
}

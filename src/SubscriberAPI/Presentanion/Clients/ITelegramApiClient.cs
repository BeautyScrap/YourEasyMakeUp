using SubscriberAPI.Domain;

namespace SubscriberAPI.Presentanion.Clients
{
    public interface ITelegramApiClient
    {
        Task SendFoundProduct(Subscription subscription);
    }
}

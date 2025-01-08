using SubscriberAPI.Domain;

namespace SubscriberAPI.Infrastructure.Clients
{
    public interface ITelegramApiClient
    {
        Task SendFoundProduct(Subscription subscription);
    }
}

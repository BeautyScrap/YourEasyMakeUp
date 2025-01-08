using SubscriberAPI.Domain;
using System.ComponentModel;

namespace SubscriberAPI.Infrastructure.Clients
{
    public interface IProductApiClient
    {
        Task<List<Subscription>> GetProducts(List<Subscription> subscriptions);
    }

}

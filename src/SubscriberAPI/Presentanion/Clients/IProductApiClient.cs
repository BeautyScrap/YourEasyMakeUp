using SubscriberAPI.Domain;
using System.ComponentModel;

namespace SubscriberAPI.Presentanion.Clients
{
    public interface IProductApiClient
    {
        Task<List<Subscription>> GetProducts(List<Subscription> subscriptions);
    }

}

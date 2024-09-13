using Microsoft.AspNetCore.SignalR;
using SubscriberAPI.Domain;

namespace SubscriberAPI.Infrastructure
{
    public interface ISubscribersRepository
    {
        Task<IEnumerable<Subscription>> GetAllSubscribersAsync();
        Task<Subscription> GetSubscriberAsync(string userId);
        Task CreateAsync(SubscriptionDto subscriptionDto);
        Task<int> UpdateAsync(Subscription newSubscriber);
        Task<int> DeleteAsync(string userId);
        Task<IEnumerable<Subscription>> GetFieldsForSearchAsync();
    }
}

using Microsoft.AspNetCore.SignalR;
using SubscriberAPI.Domain;

namespace SubscriberAPI.Infrastructure
{
    public interface ISubscribersRepository
    {
        Task<IEnumerable<Subscriber>> GetAllSubscribersAsync();
        Task<Subscriber> GetSubscriberAsync(string userId);
        Task CreateAsync(Subscriber newSubscriber);
        Task<int> UpdateAsync(Subscriber newSubscriber);
        Task<int> DeleteAsync(string userId);
    }
}

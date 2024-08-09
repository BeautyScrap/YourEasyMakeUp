using Microsoft.AspNetCore.SignalR;
using SubscriberAPI.Domain;

namespace SubscriberAPI.Infrastructure
{
    public interface ISubscribersRepository
    {
        Task<IEnumerable<Subscriber>> GetSubscribersAsync();
        Task<Subscriber> GetSubscriberAsync(string userId);
        Task Create(Subscriber newSubscriber);
        Task<bool> Delete(Subscriber newSubscriber);
    }
}

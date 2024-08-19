using SubscriberAPI.Domain;

namespace SubscriberAPI.Application
{
    public interface ISubscrieberService
    {
        Task<IEnumerable<Subscriber>> GetAllAsync();
        Task<Subscriber> GetById(string userId);
        Task Create(Subscriber newSubscriber);
        Task<bool> Update(string userId, Subscriber newSubscriber);
        Task<bool> Delete(string userId);
    }
}

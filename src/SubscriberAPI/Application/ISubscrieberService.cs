using SubscriberAPI.Domain;

namespace SubscriberAPI.Application
{
    public interface ISubscrieberService
    {
        Task<IEnumerable<SubscriptionDto>> GetAllAsync();
        Task<SubscriptionDto> GetById(string userId);
        Task Create(Subscription subscription);
        Task<bool> Update(string userId, Subscription newSubscription);
        Task<bool> Delete(string userId);
        Task<List<Subscription>> GetFieldsForSearchById();

    }
}

using SubscriberAPI.Domain;

namespace SubscriberAPI.Application
{
    public interface ISubscrieberService
    {
        Task<IEnumerable<SubscriberDto>> GetAllAsync();
        Task<SubscriberDto> GetById(string userId);
        Task Create(Subscriber newSubscriber);
        Task<bool> Update(string userId, Subscriber newSubscriber);
        Task<bool> Delete(string userId);
        Task<List<SubscriberDto>> GetFieldsForSearchById();

    }
}

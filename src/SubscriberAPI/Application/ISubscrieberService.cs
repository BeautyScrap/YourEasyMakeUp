using SubscriberAPI.Domain;

namespace SubscriberAPI.Application
{
    public interface ISubscrieberService
    {
        Task<IEnumerable<Subscription>> GetAllAsync();                                                        
        Task<Result<Subscription>> GetById(string userId);
        Task Create(Subscription subscription);
        Task<bool> Update(string userId, Subscription subscription);
        Task<bool> Delete(string userId);
        Task<List<Subscription>> GetFieldsForSearchById();
        Task<bool> UpdateStatusForFoundProduct(string userId, string name);

    }
}

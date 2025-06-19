using Microsoft.AspNetCore.SignalR;
using SubscriberAPI.Domain;

namespace SubscriberAPI.Infrastructure
{
    public interface ISubscribersRepository
    {
        Task<IEnumerable<Subscription>> GetAllSubscribersAsync(); 
        Task<Subscription> GetSubscriberAsync(string userId);// AK TODO потом переделать эти методы так, чтобы в агрементах были сами объекты,
                                                                // а не их DTo,  и возвращали методы тоже Объекты!
        Task CreateAsync(Subscription subscription);
        Task<int> UpdateAsync(Subscription subscription);
        Task<int> DeleteAsync(string userId);
        Task<IEnumerable<Subscription>> GetFieldsForSearchAsync();
        Task <int> UpdateStatusFoundProduct(string userId, string name);
    }
}

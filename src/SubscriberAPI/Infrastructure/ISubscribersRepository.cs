using Microsoft.AspNetCore.SignalR;
using SubscriberAPI.Domain;

namespace SubscriberAPI.Infrastructure
{
    public interface ISubscribersRepository
    {
        Task<IEnumerable<SubscriptionDto>> GetAllSubscribersAsync(); 
        Task<SubscriptionDto> GetSubscriberAsync(string userId);// AK TODO потом переделать эти методы так, чтобы в агрементах были сами объекты,
                                                                // а не их DTo,  и возвращали методы тоже Объекты!
        Task CreateAsync(SubscriptionDto subscriptionDto);
        Task<int> UpdateAsync(SubscriptionDto subscriptionDto);
        Task<int> DeleteAsync(string userId);
        Task<IEnumerable<SubscriptionDto>> GetFieldsForSearchAsync();
    }
}

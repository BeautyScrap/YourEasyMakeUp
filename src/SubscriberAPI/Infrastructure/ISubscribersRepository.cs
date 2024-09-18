using Microsoft.AspNetCore.SignalR;
using SubscriberAPI.Domain;

namespace SubscriberAPI.Infrastructure
{
    public interface ISubscribersRepository
    {
        Task<IEnumerable<SubscriptionDto>> GetAllSubscribersAsync(); //  тут тоже должно вернуться Dto объект, как и вдругих методах репозитория
                                                                     //  ,а в cервисе преобразовать dto уже в объект Subscription
        Task<SubscriptionDto> GetSubscriberAsync(string userId);
        Task CreateAsync(SubscriptionDto subscriptionDto);
        Task<int> UpdateAsync(SubscriptionDto subscriptionDto);
        Task<int> DeleteAsync(string userId);
        Task<IEnumerable<SubscriptionDto>> GetFieldsForSearchAsync();
    }
}

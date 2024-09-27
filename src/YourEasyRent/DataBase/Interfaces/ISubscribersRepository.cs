using YourEasyRent.Entities.ProductForSubscription;

namespace YourEasyRent.DataBase.Interfaces
{
    public interface ISubscribersRepository
    {
        Task<ProductForSubscription> GetSubscriberAsync(string UserId);
        Task CreateSubscriberAsync(ProductForSubscription subscribers);

    }
}

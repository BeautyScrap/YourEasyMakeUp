using YourEasyRent.Entities.ProductForSubscription;

namespace YourEasyRent.Services
{
    public interface IProductForSubscriptionService
    {
        Task<List<ProductForSubscription>> ProductHandler(List<ProductForSubscription> products);
    }
}

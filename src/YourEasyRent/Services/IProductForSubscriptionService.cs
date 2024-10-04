using YourEasyRent.Entities.ProductForSubscription;

namespace YourEasyRent.Services
{
    public interface IProductForSubscriptionService
    {
        Task ProductHandler(List<ProductForSubscription> products);
    }
}

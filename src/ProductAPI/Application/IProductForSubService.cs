using ProductAPI.Contracts.ProductForSubscription;
using ProductAPI.Domain.ProductForSubscription;

namespace ProductAPI.Application
{
    public interface IProductForSubService
    {
        Task<List<AvaliableProduct>> ProductHandler(List<ProductForSub> products);
    }
}

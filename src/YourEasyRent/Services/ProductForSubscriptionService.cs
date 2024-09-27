using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Entities.ProductForSubscription;

namespace YourEasyRent.Services
{
    public class ProductForSubscriptionService
    {
        private readonly ILogger<ProductForSubscriptionService> _logger;
        private readonly IProductRepository _productRepository;

        public ProductForSubscriptionService(ILogger<ProductForSubscriptionService> logger, IProductRepository repository)
        {
            _logger = logger;
            _productRepository = repository;
        }
        public async Task<IEnumerable<ProductForSubscription>> ProductHandler(ProductForSubscription product)
        {
            // AK TO DO тут смапливаем  объект product в dto, склеиваем его в list и передаем в репозиторий, котоый ищет продукты ,
            // далее преобразуем его обратно в лист из ProductForSubscription и передаем в контроллер готовый список продуктов
        }

    }

}

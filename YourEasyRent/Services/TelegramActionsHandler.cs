using Telegram.Bot;
using YourEasyRent.DataBase.Interfaces;

namespace YourEasyRent.Services
{
    public class TelegramActionsHandler : ITelegramActionsHandler
    {

        private readonly IProductRepository _productRepository;

        public TelegramActionsHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }   

        public async Task<string> GetFilteredProductsMessage(string brand, decimal price)
        {
            var products = await _productRepository.GetProductsByBrandAndPrice(brand, price);
            return products.Select(p=> $"{p.Brand}{p.Name}{p.Price}").Aggregate((a, b) => $"{a}\n{b}");
        }

        
    }
}

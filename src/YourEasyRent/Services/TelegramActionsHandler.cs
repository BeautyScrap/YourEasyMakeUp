using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using System.ComponentModel;
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

        public async Task<IEnumerable<string>> GetFilteredProductsMessage(string brand, string category) // возможно сюда можно будет передать UserId  и resultForSearch  из ROS
        {
            var products = await _productRepository.GetProductsByBrandAndCategory(brand, category);
            {
                var productStrings = products.Select(p =>
            $"*{p.Brand}*\n" +
            $"{p.Name}\n" +
            $"{p.Category}\n" +
            $"{p.Price}\n" +
            $"[.]({p.ImageUrl})\n" +
            $"[Ссылка на продукт]({p.Url})");
                return productStrings;
            }
        }
    }
}

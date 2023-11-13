using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using System.ComponentModel;
using Telegram.Bot;
using YourEasyRent.DataBase.Interfaces;

namespace YourEasyRent.Services
{
    public class TelegramActionsHandler : ITelegramActionsHandler
    {

        private readonly IProductRepository _productRepository;// вводим экземпляр _productRepository  класса IProductRepository для абстрагирования доступа (скрывает детали взаимодействия с базой данных от остальной части приложения) к данным и выполнения операций с данными(CRUD), связанными с продуктами  из базы данных MongoDb

        public TelegramActionsHandler(IProductRepository productRepository) // вводим пользовательский контсруктор для инициализации переменной  _productRepository
        {
            _productRepository = productRepository;
        }



        public async Task<IEnumerable<string>> GetFilteredProductsMessage(string brand, string category)
        {
            var products = await _productRepository.GetProductsByBrandAndCategory(brand, category);
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

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

        //public async Task<string> GetFilteredProductsMessage(string brand, decimal price)
        //{
        //    var products = await _productRepository.GetProductsByBrandAndPrice(brand, price);
        //    return products.Select(p=> $"{p.Brand}{p.Name}{p.Price}").Aggregate((a, b) => $"{a}\n{b}");
        //}

        public async Task<string> GetFilteredProductsMessage(string brand, string category)
        {
            var products = await _productRepository.GetProductsByBrandAndCategory(brand, category); 
            return products.Select(p => $"{p.Brand}{p.Name}{p.Category}").Aggregate((a, b) => $"{a}\n{b}");
        }

    }
}

using MongoDB;
using MongoDB.Driver;
using Telegram.Bot;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Entities;

namespace YourEasyRent.DataBase
{
    public class TelegramRepository : ITelegramRepository
    {

        private readonly IMongoCollection<Product> _product;  // объявляет приватное поле _product в текущем классе TelegramRepository, которое будет использоваться для внедрения зависимости ITelegramRepository. Это поле будет доступно для использования в методах и конструкторах класса

        public TelegramRepository(DataBaseConfig configuration, IMongoClient client) // объявляем конструктор. Конструктор принимает два аргумента: DataBaseConfig configuration и IMongoClient client. Класс DataBaseConfig используется для передачи конфигурационных данных, а IMongoClient представляет клиент MongoDB, который используется для установления соединения с базой данных.
        {
            var database = client.GetDatabase(configuration.DataBaseName);
            _product = database.GetCollection<Product>(configuration.CollectionName);
        }
        public async Task<IEnumerable<Product>> GetProductsByBrandAndPrice(string brand, decimal price)
        {
            var filter = Builders<Product>.Filter.And
                (Builders<Product>.Filter.Eq(_ => _.Brand, brand),
                 Builders<Product>.Filter.Gte(_ => _.Price, price),  
                 Builders<Product>.Filter.Lte(_ => _.Price, price));
            var products = await _product.Find(filter).ToListAsync();
            return products;    
           

        }
    }
}

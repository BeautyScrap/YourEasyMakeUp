using MongoDB;
using MongoDB.Driver;
using YourEasyRent.Entities;



namespace YourEasyRent.DataBase
{
    public class CatalogContext //   публичный класс ,который представляет контекст базы данных для работы с коллекцией Products.
    {
        public CatalogContext(DataBaseConfig configuration, IMongoClient client) //вводим конструктор класса CatalogContext. Конструктор принимает два аргумента: DataBaseConfig configuration и IMongoClient client. Класс DataBaseConfig используется для передачи конфигурационных данных, а IMongoClient представляет клиент MongoDB, который используется для установления соединения с базой данных.
        { 
            var database = client.GetDatabase(configuration.DataBaseName ); //переменная database инициализируется с помощью метода GetDatabase, вызываемого из объекта client, и передается имя базы данных из объекта configuration.
            Products = database.GetCollection<Product>(configuration.CollectionName); // GetCollection<Product> - это метод, который возвращает коллекцию объектов типа Product.

        }

        public IMongoCollection<Product> Products { get; }
    }
}

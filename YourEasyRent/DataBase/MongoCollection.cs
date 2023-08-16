using MongoDB;
using MongoDB.Driver;
using YourEasyRent.Entities;



namespace YourEasyRent.DataBase
{
    public class MongoCollection // заменила на mongocollection  публичный класс ,который представляет контекст базы данных для работы с коллекцией Products.
    {
        public MongoCollection(DataBaseConfig configuration, IMongoClient client) //вводим конструктор класса CatalogContext. Конструктор принимает два аргумента: DataBaseConfig configuration и IMongoClient client. Класс DataBaseConfig используется для передачи конфигурационных данных, а IMongoClient представляет клиент MongoDB, который используется для установления соединения с базой данных.
        {
            //var mongoClient = new MongoClient(configuration.ConnectionString); обычно вводиться еще mongoClient, стоит ли его вводить мне??
            var database = client.GetDatabase(configuration.DataBaseName ); //переменная database инициализируется с помощью метода GetDatabase, вызываемого из объекта client, и передается имя базы данных из объекта configuration.
            Products = database.GetCollection<Product>(configuration.CollectionName); // GetCollection<Product> - это метод, который возвращает коллекцию объектов типа Product.

        }

        public IMongoCollection<Product> Products { get; } //добавила операторы для CRUD

        public async Task<List<Product>> GetAsync() =>
            await Products.Find(_ => true).ToListAsync();

        public async Task<Product?> GetAsync(string id) =>
            await Products.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Product newProduct) =>
            await Products.InsertOneAsync(newProduct);

        public async Task UpdateAsync(string id, Product updatedProduct) =>
            await Products.ReplaceOneAsync(x => x.Id == id, updatedProduct);

        public async Task RemoveAsync(string id) =>
            await Products.DeleteOneAsync(x => x.Id == id);

    }
}

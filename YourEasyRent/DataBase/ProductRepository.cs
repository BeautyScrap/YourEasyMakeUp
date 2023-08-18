using MongoDB;
using MongoDB.Driver;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Entities;



namespace YourEasyRent.DataBase
{
    public class ProductRepository: IProductRepository // заменила на mongocollection  публичный класс ,который представляет контекст базы данных для работы с коллекцией Products.

    {
        
        private readonly IMongoCollection<Product> _products;   
       

        public ProductRepository(DataBaseConfig configuration, IMongoClient client) //вводим конструктор класса CatalogContext. Конструктор принимает два аргумента: DataBaseConfig configuration и IMongoClient client. Класс DataBaseConfig используется для передачи конфигурационных данных, а IMongoClient представляет клиент MongoDB, который используется для установления соединения с базой данных.
        {
            //var mongoClient = new MongoClient(configuration.ConnectionString); обычно вводиться еще mongoClient, стоит ли его вводить мне??
            var database = client.GetDatabase(configuration.DataBaseName ); //переменная database инициализируется с помощью метода GetDatabase, вызываемого из объекта client, и передается имя базы данных из объекта configuration.
            _products = database.GetCollection<Product>(configuration.CollectionName); // GetCollection<Product> - это метод, который возвращает коллекцию объектов типа Product.

        }

        public Task Create(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> CreateMany(IEnumerable<Product> products)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetBrand(string brand)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetById(string id)
        {
            return (IEnumerable<Product>)await _products.Find(_ => _.Id == id).FirstOrDefaultAsync();
        }

        public Task<IEnumerable<Product>> GetByName(int name)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _products.Find(_ => true).ToListAsync();
        }

        public Task<bool> Update(Product product)
        {
            throw new NotImplementedException();
        }


    }
}

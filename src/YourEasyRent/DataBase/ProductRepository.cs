using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Entities;
using MongoDB.Driver.Linq;
using YourEasyRent.TelegramMenu;
using YourEasyRent.UserState;



namespace YourEasyRent.DataBase
{
    public class ProductRepository : IProductRepository  

    {
        private readonly IMongoCollection<Product> _productCollection;//  вводим экземпляр  _productCollection класса IMongoCollection дла работы с базой данных

        public ProductRepository(DataBaseConfig configuration, IMongoClient client) //вводим конструктор класса CatalogContext. Конструктор принимает два аргумента: DataBaseConfig configuration и IMongoClient client. Класс DataBaseConfig используется для передачи конфигурационных данных, а IMongoClient представляет клиент MongoDB, который используется для установления соединения с базой данных.
        {
            var database = client.GetDatabase(configuration.DataBaseName); //переменная database инициализируется с помощью метода GetDatabase, вызываемого из объекта client, и передается имя базы данных из объекта configuration.
            _productCollection = database.GetCollection<Product>("Products") ??  throw new ArgumentNullException(nameof(_productCollection)); ; // GetCollection<Product> - это метод, который возвращает коллекцию объектов типа Product.
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _productCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Product> Get(string id)
        {
            return await _productCollection.Find(_ => _.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetByBrand(string brand)
        {
            return await _productCollection.Find(_ => _.Brand == brand).ToListAsync(); // обращеемся к объекту _ и используем его своейство Brand, где свойство нашего объекта Brand будет равно параметру Brand
        }

        public async Task<Product> GetByName(string name)
        {
            return await _productCollection.Find(_ => _.Name == name).FirstOrDefaultAsync();
        }

        public async Task Create(Product newproduct)
        {
            await _productCollection.InsertOneAsync(newproduct);
        }

        public async Task<IEnumerable<string>> CreateMany(IEnumerable<Product> products)
        {
            await _productCollection.InsertManyAsync(products);
            var createdIds = products.Select(p => p.Id); // - Здесь создается коллекция идентификаторов продуктов, используя LINQ-запрос.
            return createdIds;
        }

        public async Task<bool> Update(Product updateProduct)
        {
            var filter = Builders<Product>.Filter.And(
                Builders<Product>.Filter.Eq(_ => _.Brand, updateProduct.Brand),
                Builders<Product>.Filter.Eq(_ => _.Name, updateProduct.Name));
            var updateResult = await _productCollection.ReplaceOneAsync(filter, updateProduct);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<Product>.Filter.Eq(_ => _.Id, id);
            var deleteProduct = await _productCollection.DeleteOneAsync(filter);
            return deleteProduct.IsAcknowledged && deleteProduct.DeletedCount > 0;
        }

        public async Task<IEnumerable<Product>> GetProductsByBrandAndCategory(List<string> listWithResult)
        {
            var filter = Builders<Product>.Filter.And
                (Builders<Product>.Filter.Eq(_ => _.Brand, listWithResult[0]),
                 Builders<Product>.Filter.Eq(_ => _.Category, listWithResult[1]));

            var products = await _productCollection.Find(filter).ToListAsync();
            return products;
        }

        public async Task UpsertProduct(Product product)
        {
            var filter = Builders<Product>.Filter.And
                (Builders<Product>.Filter.Eq(_ => _.Brand, product.Brand),
                Builders<Product>.Filter.Eq(_ => _.Name, product.Name));

            var update = Builders<Product>.Update
                .Set(_ => _.Price, product.Price)
                .Set(_ => _.Url, product.Url);

            var options = new UpdateOptions
            {
                IsUpsert = true
            };
            await _productCollection.UpdateOneAsync(filter, update, options);

        }

        public async Task UpsertManyProducts(IEnumerable<Product> products)
        {
            foreach(var product in products) 
            { 
                await UpsertProduct(product);  
            }
        }

        public async Task<List<string>> GetBrandForMenu(int limit)
        {
            var res = await _productCollection
                .AsQueryable()
                .Select(x => x.Brand)
                .Take(limit)
                .Distinct()
                .ToListAsync();

            return res;
        }

        public async Task<Product> GetProductsToSearchForPriceBrandName(Product productForSearch)
        {
            var filter = Builders<Product>.Filter.And(
                Builders<Product>.Filter.Eq(_ => _.Brand, productForSearch.Brand),
                Builders<Product>.Filter.Eq(_ => _.Name, productForSearch.Name),
                Builders<Product>.Filter.Lt(_ => _.Price, productForSearch.Price));
            var product = await _productCollection.Find<Product>(filter).FirstOrDefaultAsync();
            return product;
        }
    }
}

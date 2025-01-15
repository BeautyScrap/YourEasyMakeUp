using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProductAPI.Domain.Product;
using ProductAPI.Domain.ProductForSubscription;
using ProductAPI.Domain.ProductForUser;
using ProductAPI.Infrastructure.Persistence;

namespace ProductAPI.Infrastructure
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _productCollection;//  вводим экземпляр  _productCollection класса IMongoCollection дла работы с базой данных
        private readonly ILogger<ProductRepository> _logger;
        public ProductRepository(MongoDBSettings configuration, IMongoClient client, ILogger<ProductRepository> logger) //вводим конструктор класса CatalogContext. Конструктор принимает два аргумента: DataBaseConfig configuration и IMongoClient client. Класс DataBaseConfig используется для передачи конфигурационных данных, а IMongoClient представляет клиент MongoDB, который используется для установления соединения с базой данных.
        {
            var database = client.GetDatabase(configuration.DatabaseName); //переменная database инициализируется с помощью метода GetDatabase, вызываемого из объекта client, и передается имя базы данных из объекта configuration.
            _productCollection = database.GetCollection<Product>("Products") ?? throw new ArgumentNullException(nameof(_productCollection)); ; // GetCollection<Product> - это метод, который возвращает коллекцию объектов типа Product.
            _logger = logger;
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
            return await _productCollection.Find(_ => _.Brand == brand).ToListAsync();
        }

        public async Task<Product> GetByName(string name)
        {
            return await _productCollection.Find(_ => _.Name == name).FirstOrDefaultAsync();
        }

        public async Task Create(Product newproduct)
        {
            await _productCollection.InsertOneAsync(newproduct);
        }

        public async Task CreateMany(IEnumerable<Product> products)
        {
            await _productCollection.InsertManyAsync(products);
            await DeleteDuplicate(); 

                                                                
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
            await DeleteDuplicate();
        }

        public async Task UpsertManyProducts(IEnumerable<Product> products)
        {
            foreach (var product in products)
            {
                await UpsertProduct(product);
            }
        }

        public async Task<List<string>> GetBrandForMenu()
        {
            var res = await _productCollection
                .AsQueryable()
                .Select(x => x.Brand)
                .Take(10)
                .Distinct()
                .ToListAsync();
            return res;
        }

        public async Task<AvaliableProductDto?> GetProductForOneSubscriber(ProductForSubDto productForSearch)
        {
            double roundedPrice = Math.Round((double)productForSearch.Price, 2);
            var filter = Builders<Product>.Filter.And(
                    Builders<Product>.Filter.Eq(_ => _.Name, productForSearch.Name),
                    Builders<Product>.Filter.Lt(_ => (double)_.Price, roundedPrice));
            var product = await _productCollection.Find(filter).FirstOrDefaultAsync();
            if (product == null)
            {
                return null;
            }
            var result = new AvaliableProductDto()
            {
                Brand = product.Brand,
                Name = product.Name,
                Price = product.Price,
                Url = product.Url,
                UrlImage = product.ImageUrl
            };
            return result;
        }

        public async Task<IEnumerable<AvaliableResultForUserDto>> GetProductResultForUser(ProductResultForUserDto productForUser)
        {
            var filter = Builders<Product>.Filter.And(
                Builders<Product>.Filter.Eq(_ => _.Brand, productForUser.Brand),
                Builders<Product>.Filter.Eq(_ => _.Category, productForUser.Category));
            var products = await _productCollection.Find(filter).ToListAsync();
            var results = new List<AvaliableResultForUserDto>();
            foreach(var product in products)
            {
                var productResult = new AvaliableResultForUserDto()
                {
                    Brand = product.Brand,
                    Name = product.Name,
                    Category = product.Category,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Url = product.Url
                };
                results.Add(productResult);
            }
            return results;
        }

        public async Task DeleteDuplicate()
        {
            var groups = await _productCollection.Aggregate()
                .Group(p => new { p.Name }, g => new
                {
                    Name = g.Key,
                    Count = g.Count(),
                    Ids = g.Select(x => x.Id).ToList()
                }).ToListAsync();
            var duplicateGroups = groups.Where(g => g.Count > 1);
            foreach (var group in duplicateGroups)
            {
                var idsToDelete = group.Ids.Skip(1).ToList();
                var filter = Builders<Product>.Filter.In(p => p.Id, idsToDelete);
                await _productCollection.DeleteManyAsync(filter);
            }
        }

        public async Task<AvaliableResultForUserDto> GetOneProductResultForUser(ProductResultForUserDto productForUser) // использую этот метод
        {
            var filter = Builders<Product>.Filter.And(
                Builders<Product>.Filter.Eq(_ => _.Brand, productForUser.Brand),
                Builders<Product>.Filter.Eq(_ => _.Category, productForUser.Category));
            var product = await _productCollection.Find(filter).FirstOrDefaultAsync();
                var productResult = new AvaliableResultForUserDto()
                {
                    Brand = product.Brand,
                    Name = product.Name,
                    Category = product.Category,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Url = product.Url
                };
            return productResult;
        }
    }
}

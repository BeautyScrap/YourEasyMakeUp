using MongoDB;
using MongoDB.Driver;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Entities;



namespace YourEasyRent.DataBase
{
    public class ProductRepository: IProductRepository // заменила на mongocollection  публичный класс ,который представляет контекст базы данных для работы с коллекцией Products.

    {
      
        private readonly IMongoCollection<Product> _product;   
       

        public ProductRepository(DataBaseConfig configuration, IMongoClient client) //вводим конструктор класса CatalogContext. Конструктор принимает два аргумента: DataBaseConfig configuration и IMongoClient client. Класс DataBaseConfig используется для передачи конфигурационных данных, а IMongoClient представляет клиент MongoDB, который используется для установления соединения с базой данных.
        {
            //var mongoClient = new MongoClient(configuration.ConnectionString); обычно вводиться еще mongoClient, стоит ли его вводить мне??
            var database = client.GetDatabase(configuration.DataBaseName ); //переменная database инициализируется с помощью метода GetDatabase, вызываемого из объекта client, и передается имя базы данных из объекта configuration.
            _product = database.GetCollection<Product>(configuration.CollectionName); // GetCollection<Product> - это метод, который возвращает коллекцию объектов типа Product.

        }

        // метод GET
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _product.Find(_ => true).ToListAsync();
        }
        public async Task<Product> Get(string id)
        {
            return await _product.Find(_ => _.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetByBrand(string brand)
        {
            return await _product.Find(_ => _.Brand == brand).ToListAsync(); // обращеемся к объекту _ и используем его своейство Brand, где свойство нашего объекта Brand будет равно параметру Brand
        }
       

        public async Task<Product> GetByName(string name)
        {
            return await _product.Find(_ => _.Name == name).FirstOrDefaultAsync();
        }

        //метод POST
        public async Task Create(Product newproduct)
        {
            await _product.InsertOneAsync(newproduct);  
        }

        public async Task<IEnumerable<string>> CreateMany(IEnumerable<Product> products)
        {
            await _product.InsertManyAsync(products);

            var createdIds = products.Select(p => p.Id); // - Здесь создается коллекция идентификаторов продуктов, используя LINQ-запрос.

            return createdIds;
        }
       
        // метод PUT

        public async Task<bool> Update(string id, Product updateProduct) // метод Update возвращает значение типа bool, указывающее на успешность операции обновления. Если операция обновления прошла успешно, метод вернет true
        {
            var filter = Builders<Product>.Filter.Eq(_ => _.Id, id); // создаю фильтр для поиска продукта по его идентификатору.
            var updateResult = await _product.ReplaceOneAsync(filter, updateProduct);// выполняет замену документа с учетом заданного фильтра.
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;//возвращает true, если и операция была успешно подтверждена сервером (acknowledged), и хотя бы один документ был изменен в результате операции обновления.

        }

        //метод DELETE
        public async Task<bool> Delete(string id)
        {
            
            var filter = Builders<Product>.Filter.Eq(_ => _.Id, id);
            var deleteProduct = await _product.DeleteOneAsync(filter);//  Выполняет удаление элемента с учетом задаанного фильтра
            return deleteProduct.IsAcknowledged && deleteProduct.DeletedCount > 0;
                    
        }


    }
}

using MongoDB;
using MongoDB.Driver;
using YourEasyRent.Entities;

namespace YourEasyRent.DataBase
{
    public interface ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }  
    }
}

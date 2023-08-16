using MongoDB;
using MongoDB.Driver;
using YourEasyRent.Entities;

namespace YourEasyRent.DataBase
{
    public interface IMongoCollection
    {
        public IMongoCollection<Product> Products { get; }  
    }
}

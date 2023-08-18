using MongoDB.Driver;
using YourEasyRent.Entities;

namespace YourEasyRent.DataBase.Interfaces
{
    public interface IProductRepository
    {
        // сделать сигнатуры как было в iproductrepositor

        Task<IEnumerable<Product>> GetProducts();//добавила операторы для CRUD  к IMongoCollection
        Task<IEnumerable<Product>> GetById(string id);
        Task<IEnumerable<Product>> GetBrand(string brand);
        Task<IEnumerable<Product>> GetByName(int name);
        Task Create(Product product);
        Task<IEnumerable<string>> CreateMany(IEnumerable<Product> products);
        Task<bool> Update(Product product);
        Task<bool> Delete(string id);



    }
}

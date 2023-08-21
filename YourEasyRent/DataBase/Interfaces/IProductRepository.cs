using MongoDB.Driver;
using YourEasyRent.Entities;

namespace YourEasyRent.DataBase.Interfaces
{
    public interface IProductRepository
    {
        // сделать сигнатуры как было в iproductrepositor

        Task<IEnumerable<Product>> GetProducts();//добавила операторы для CRUD  к IMongoCollection, метод GET
        Task<Product> GetById(string id);
        Task<Product> GetByBrand(string brand);
        Task<Product> GetByName(string name);
        Task Create(Product newProduct); // метод POST
        Task<IEnumerable<string>> CreateMany(IEnumerable<Product> products); // метод  POST
        Task<bool> Update(Product product); // метод PUT
        Task<bool> Delete(Product deleteProduct); // метод DELETE



    }
}

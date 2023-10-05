using MongoDB.Driver;
using YourEasyRent.Entities;

namespace YourEasyRent.DataBase.Interfaces
{
    public interface IProductRepository
    {
        // сделать сигнатуры как было в iproductrepositor

        Task<IEnumerable<Product?>> GetProducts();//добавила операторы для CRUD  к IMongoCollection, метод GET
        Task<Product> Get(string id);
        Task<IEnumerable<Product>> GetByBrand(string brand);
        
        Task<IEnumerable<Product>> GetByBrandAndCategory(string brand, string category);
        Task<Product> GetByName(string name);
        Task Create(Product newProduct); // метод POST
        Task<IEnumerable<string>> CreateMany(IEnumerable<Product> products); // метод  POST
        Task<bool> Update(string id, Product updateproduct); // метод PUT
        Task<bool> Delete(string id); // метод DELETE
       
    }
}

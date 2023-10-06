using MongoDB.Driver;
using YourEasyRent.Entities;

namespace YourEasyRent.DataBase.Interfaces
{
    public interface IProductRepository
    {
        
        Task<IEnumerable<Product?>> GetProducts();
        Task<Product> Get(string id);
        Task<IEnumerable<Product>> GetByBrand(string brand);
        Task<Product> GetByName(string name);
        Task Create(Product newProduct); 
        Task<IEnumerable<string>> CreateMany(IEnumerable<Product> products); 
        Task<bool> Update(string id, Product updateproduct); 
        Task<bool> Delete(string id);
        Task<IEnumerable<Product>> GetProductsByBrandAndPrice(string brand, decimal price);

    }
}

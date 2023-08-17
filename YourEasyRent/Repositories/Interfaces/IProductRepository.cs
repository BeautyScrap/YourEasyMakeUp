using YourEasyRent.Entities;

namespace YourEasyRent.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts(); // IEnumerable - получение коллекции элементов 
        Task<Product> Get(string id);
        Task<IEnumerable<Product>> GetByBrand(string brand);
        Task<IEnumerable<Product>> GetByName(string name);
        Task<IEnumerable<Product>> GetByCategory(string category);
        Task Create(Product product);
        Task<IEnumerable<string>> CreateMany(IEnumerable<Product> products);
        Task<bool> Update(Product product); // bool- если запить будет добвлена, то возвращается true,  если нет, то false
        Task<bool> Delete(Product product);   







    }
}

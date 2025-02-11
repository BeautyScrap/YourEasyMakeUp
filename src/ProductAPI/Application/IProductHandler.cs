using ProductAPI.Domain.Product;

namespace ProductAPI.Application
{
    public interface IProductHandler
    {        
        Task CreateManyProductAsync(IEnumerable<Product> products);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductAsync();
        Task UpsertProduct(Product product);// возможно этот метод и не нужен будет
        Task<bool> UpsertManyProducts(IEnumerable<Product> products);
        Task<bool> DeleteProductAsync(string name);


    }

}

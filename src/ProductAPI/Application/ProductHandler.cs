using ProductAPI.Domain.Product;
using ProductAPI.Infrastructure;

namespace ProductAPI.Application
{
    public class ProductHandler : IProductHandler
    {

        private readonly IProductRepository _repository;
        public ProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }
        public async Task CreateManyProductAsync(IEnumerable<Product> products)
        {
            await _repository.CreateMany(products);
        }

        public async Task<bool> UpsertManyProducts(IEnumerable<Product> products)
        {
            var result = await _repository.UpdateManyProducts(products);
            if (result > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var result = await _repository.GetProducts();
            return result.ToList();
        }
        public async Task<bool> DeleteProductAsync(string name)
        {
            var result = await _repository.Delete(name);
            if (result > 0)
            {
                return true;
            }
            return false;
        }

    public Task<Product> GetProductAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpsertProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}

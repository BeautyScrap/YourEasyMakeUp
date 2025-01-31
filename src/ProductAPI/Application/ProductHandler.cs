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
        public async Task CreateManyProductAsync(IEnumerable<Product> products)// AK TODO вопрос:до какого уровня пробрасывать лист с продуктами? или в репозиторий тоже кидать список с продуктами  dto?
        {
            foreach (var product in products) 
            { 
                var dto = product.ToDto();
                await _repository.CreateAsync(dto);// AK TODO lastUpdate -  потестировать как теперь продукты сохраняются в новую базу
            }
        }

        public Task<bool> DeleteProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpsertManyProducts(IEnumerable<Product> products)
        {
            throw new NotImplementedException();
        }

        public Task UpsertProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}

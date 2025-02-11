using ProductAPI.Domain.Product;
using ProductAPI.Domain.ProductForSubscription;
using ProductAPI.Domain.ProductForUser;

namespace ProductAPI.Infrastructure
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        //Task<Product> Get(string id);
        //Task<IEnumerable<Product>> GetByBrand(string brand);
        //Task<Product> GetByName(string name);
        //Task CreateAsync(Product product);
        Task CreateMany(IEnumerable<Product> products);
        //Task<bool> Update(Product updateproduct);
        Task<int> Delete(string name);      
        //Task UpsertProduct(Product product);
        Task<int> UpdateManyProducts(IEnumerable<Product> products);
        Task<List<string>> GetBrands();// AK TODO вопрос Если нет дополнительной логики, просто отставить репозиторий в контроллере!
        //
        //Task <IEnumerable<AvaliableResultForUserDto>> GetProductResultForUser(ProductResultForUserDto productForUser);
        Task <AvaliableResultForUser> GetOneProductResultForUser(ProductResultForUser productForUser);
        //Task<AvaliableProductDto> GetProductForOneSubscriber(ProductForSubDto productForSearch);
        //Task DeleteDuplicate();
    }
}

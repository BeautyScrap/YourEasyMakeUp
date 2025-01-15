using ProductAPI.Domain.Product;
using ProductAPI.Domain.ProductForSubscription;
using ProductAPI.Domain.ProductForUser;

namespace ProductAPI.Infrastructure
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product?>> GetProducts();
        Task<Product> Get(string id);
        Task<IEnumerable<Product>> GetByBrand(string brand);
        Task<Product> GetByName(string name);
        Task Create(Product newProduct);
        Task CreateMany(IEnumerable<Product> products);
        Task<bool> Update(Product updateproduct);
        Task<bool> Delete(string id);      
        Task UpsertProduct(Product product);
        Task UpsertManyProducts(IEnumerable<Product> products);
        Task<List<string>> GetBrandForMenu();// AK TODO вопрос Не знаю тут нужна какая то прослойка ввиде сервиса,
                                             // перед тем как сделать запрос в базу? по идеи да. тк я хочу в базу для телеграма 2 раза  и вроде как надо возвращать Dto. Может засунуть этот метод в  ProductForUserService тоже?
        Task <IEnumerable<AvaliableResultForUserDto>> GetProductResultForUser(ProductResultForUserDto productForUser);
        Task <AvaliableResultForUserDto> GetOneProductResultForUser(ProductResultForUserDto productForUser);
        Task<AvaliableProductDto> GetProductForOneSubscriber(ProductForSubDto productForSearch);
        Task DeleteDuplicate();
    }
}

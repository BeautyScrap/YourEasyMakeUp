using ProductAPI.Domain.ProductForUser;
using System.Threading.Tasks;

namespace ProductAPI.Application
{
    public interface IProductForUserService
    {
        Task<List<AvaliableResultForUser>> Handler(ProductResultForUser product);
        Task<AvaliableResultForUser> HandlerOne(ProductResultForUser product);
        Task<List<string>> GetBrandForMenu();

    }
}

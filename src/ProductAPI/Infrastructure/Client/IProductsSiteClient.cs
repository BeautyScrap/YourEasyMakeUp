using ProductAPI.Domain.Product;

namespace ProductAPI.Infrastructure.Client
{
    public interface IProductsSiteClient
    {
        Site Site { get; }
        Task<IEnumerable<Product>> FetchFromSectionAndPage(Section section, int pageNumber);
    }
}

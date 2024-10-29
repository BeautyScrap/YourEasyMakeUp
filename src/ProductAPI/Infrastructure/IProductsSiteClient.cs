using ProductAPI.Domain.Product;

namespace ProductAPI.Infrastructure
{
    public interface IProductsSiteClient
    {    
        Site Site { get; }
        Task<IEnumerable<Product>> FetchFromSectionAndPage(Section section, int pageNumber);
    }
}

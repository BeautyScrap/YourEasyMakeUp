using YourEasyRent.Entities;

namespace YourEasyRent.Services
{
    public interface IProductsSourceClient

    {
        Section Section { get; }
        
         
        Task<IEnumerable<Product>> FetchFromSection(Section section, int pageNumber);

    }
}

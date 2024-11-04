using MongoDB.Driver;

namespace ProductAPI.Domain.ProductForUser
{
    public class ProductResultForUser
    {
        public string Brand {  get;  private set; }
        public string Category { get; private set; }
        public ProductResultForUser() { }

        public static ProductResultForUser CreateProductForSearch(string brand, string category) 
        { 
            ProductResultForUser productResult = new ProductResultForUser()
            {
                Brand = brand,
                Category = category
            };
            return productResult;
        }
        public ProductResultForUserDto ToDto()
        {
            ProductResultForUserDto Dto = new ProductResultForUserDto()
            {
                Brand = Brand,
                Category = Category
            };
            return Dto;
        }
    }   
}

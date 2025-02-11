using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProductAPI.Domain.Product
{
    public class Product
    {
        public string? Id { get; private set; }
        public Site? Site { get; private set; }
        public string Brand { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string Category { get; private set; }
        public string Url { get; private set; }
        public string ImageUrl { get; private set; }

        public Product()
        {
        }
        public static Product CreateProduct(Site site, string brand, string name, decimal price, string category, string url, string imageUrl)
        {
            Product product = new Product() 
            {
                Site = site,
                Brand = brand,
                Name = name,
                Price = price,
                Category = category,
                Url = url,
                ImageUrl = imageUrl
            };
            return product;
        }

        public ProductDto ToDto()
        {
            ProductDto dto = new ProductDto()
            { 
                Site = Site.Value,// AK TODO может быть Value  подойдет, хотя наверно нет
                Brand = Brand,
                Name = Name,
                Price = Price,
                Category = Category,
                Url = Url,  
                ImageUrl=ImageUrl
            };
            return dto;
        }
    }
}

using System.Xml.Linq;
using System;

namespace ProductAPI.Domain.ProductForSubscription
{
    public class AvaliableProduct
    {
        public string UserId { get; private set; }
        public string Brand { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string Url { get; private set; }
        public AvaliableProduct() { }

        public static AvaliableProduct FromDto(string userId, AvaliableProductDto dto)
        {
            AvaliableProduct product = new AvaliableProduct()
            {
                UserId = userId,
                Brand = dto.Brand,
                Name = dto.Name,
                Price = dto.Price,
                Url = dto.Url
            };
            return product;
        }
    }
}

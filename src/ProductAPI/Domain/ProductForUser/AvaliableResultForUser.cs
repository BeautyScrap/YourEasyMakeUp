using ProductAPI.Domain.ProductForSubscription;

namespace ProductAPI.Domain.ProductForUser
{
    public class AvaliableResultForUser
    {
        public string Brand { get; private set; } 
        public string Name { get; private set; }
        public string Category { get; private set; }
        public decimal Price { get; private set; }
        public string ImageUrl { get; private set; }
        public string Url { get; private set; }
        public AvaliableResultForUser() { }

        public static AvaliableResultForUser FromDto(AvaliableResultForUserDto dto)
        {
            AvaliableResultForUser product = new AvaliableResultForUser()
            {
                Brand = dto.Brand,
                Name = dto.Name,
                Category = dto.Category,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                Url = dto.Url
            };
            return product;
        }
    }
}

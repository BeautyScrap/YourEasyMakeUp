using MongoDB.Bson.Serialization.Attributes;
using YourEasyRent.UserState;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace YourEasyRent.Entities.ProductForSubscription
{
    public class ProductForSubscription
    {

        public string UserId { get; private set; }
        public string? ChatId { get; private set; }
        public string Brand { get; private set; }
        public string Name { get; private set; }

        public decimal Price { get; private set; }
        public string? Url { get; private set; }
        public ProductForSubscription() { }

        public static ProductForSubscription TransferDataToSubscriber(UserSearchState userSearchState, List<string> intermadiateResultList) // переносим одни данные в оъбект Subscriber
        { 
            ProductForSubscription subscriber = new ProductForSubscription()
            {
                UserId = userSearchState.UserId,
                ChatId = userSearchState.ChatId,
                Brand = intermadiateResultList[0],
                Name = intermadiateResultList[1],
                Price = decimal.Parse(intermadiateResultList[2]),
                Url = intermadiateResultList[3]
            };
            return subscriber;
        }
        public static ProductForSubscription GlueResultOfSearch(string userId, string chatId, ProductForSubscriptionDto dto)
        {
            ProductForSubscription product = new ProductForSubscription()
            {
                UserId = userId,
                ChatId = chatId,
                Brand = dto.Brand,
                Name = dto.Name,
                Price = dto.Price,
                Url = dto.Url
            };
            return product;
        }

        public static ProductForSubscription CreateProductForSearch(string userId, string brand, string name, decimal price)
        {
            ProductForSubscription product = new ProductForSubscription()
            {
                UserId = userId,
                Brand = brand,
                Name = name,
                Price = price,
            };
            return product;
        }

        public override string ToString() 
        {
            return $"Brand: {Brand}, Name: {Name}, Price: {Price}, Url: {Url ?? "N/A"}";
        }
 

        public ProductForSubscriptionDto ToDto()
        {
            ProductForSubscriptionDto subscribersDto = new ProductForSubscriptionDto()
            {
                UserId = UserId,
                ChatId = ChatId,
                Brand = Brand,
                Name = Name,
                Price = Price,
                Url = Url
            };
            return subscribersDto;
        }

    }
}

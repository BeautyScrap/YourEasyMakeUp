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
        //public ProductForSubscription(ProductForSubscriptionDto subDto)
        //{
        //    UserId = subDto.UserId;
        //    ChatId = subDto.ChatId;
        //    Brand = subDto.Brand;
        //    Name = subDto.Name;
        //    Price = subDto.Price;
        //    Url = subDto.Url;
        //}


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

        public static ProductForSubscription CreateProductForSearch(string userId, string brand, string name, decimal price)
        {
            ProductForSubscription product = new ProductForSubscription()
            {
                UserId = userId,
                Brand = brand,
                Name = name,
                Price = price
            };
            return product;
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

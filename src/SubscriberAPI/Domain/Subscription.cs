using SubscriberAPI.Contracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SubscriberAPI.Domain
{
    public class Subscription
    {
        public string Id { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; private set; }
        public string ChatId { get; private set; }
        public string Brand { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string Url { get; private set; }
        public Subscription() 
        { 

        }

        public static Subscription CreateNewSubscription(string userId, string chatId, string brand, string name, decimal price, string url)
        {
            var subscription = new Subscription
            {
                UserId = userId,
                ChatId = chatId,
                Brand = brand,
                Name = name,
                Price = price,
                Url = url
            };
            return subscription;
        }

        public SubscriptionDto ToDBRepresentation()
        {
            SubscriptionDto subscribersDto = new SubscriptionDto()
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

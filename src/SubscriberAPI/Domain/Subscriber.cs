using System.Text.Json;
using System.Text.Json.Serialization;

namespace SubscriberAPI.Domain
{
    public class Subscriber
    {
        public string? Id { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get;  set; }
        public string? ChatId { get; set; }
        public string? Brand { get;  set; }
        public string? Name { get;  set; }

        public decimal? Price { get; set; }
        public string? Url { get; set; }

        public Subscriber() { }
        public Subscriber(SubscriberDto subDto)
        {
            UserId = subDto.UserId;
            ChatId = subDto.ChatId;
            Brand = subDto.Brand;
            Name = subDto.Name;
            Price = subDto.Price;
            Url = subDto.Url;
        }


        public SubscriberDto RepresentationSubscriber()
        {
            SubscriberDto subscribersDto = new SubscriberDto()
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

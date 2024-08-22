using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SubscriberAPI.Domain
{
    public class Subscriber
    {
        public string? Id { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; private set; }
        public string? ChatId { get; private set; }
        public string? Brand { get; private set; }
        public string? Name { get; private set; }

        public decimal? Price { get; private set; }
        public string? Url { get; private set; }

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


        public SubscriberDto CreateSubscriberDtoObject()
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

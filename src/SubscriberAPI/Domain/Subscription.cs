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
        public string? ChatId { get; private set; }
        public string? Brand { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string? Url { get; private set; }
        public string? UrlImage { get; private set; }// AK TODO Сюда еще можно добавить поле с Enum статусами  и метод, который будет задавать эти статусы
        public Subscription() {}
        public static Subscription CreateNewSubscription(string userId, string? chatId, string? brand, string name, decimal price)
        {
            var subscription = new Subscription
            {
                UserId = userId,
                ChatId = chatId,
                Brand = brand,
                Name = name,
                Price = price,
            };
            return subscription;
        }
        public static Subscription CreateProductforSub(string userId, string? brand, string name, decimal price, string url, string urlImage) //AK TODO вопрос: Считается ли этот объект подписчиком, если у него воявляются новые поля, когда я нахожу этот объект в другом сервисе?
                                                                                                                                              //Или можно использовать второй статический метод "CreateProductforSub"
        {
            var subscription = new Subscription
            {
                UserId = userId,
                Brand = brand,
                Name = name,
                Price = price,
                Url = url,
                UrlImage = urlImage
            };
            return subscription;
        }

        public SubscriptionDto ToDto()
        {
            SubscriptionDto subscribersDto = new SubscriptionDto()
            {
                UserId = UserId,
                ChatId = ChatId,
                Brand = Brand,
                Name = Name,
                Price = Price,
                Url = Url,
                UrlImage = UrlImage,
            };
            return subscribersDto;
        }     
    }
}

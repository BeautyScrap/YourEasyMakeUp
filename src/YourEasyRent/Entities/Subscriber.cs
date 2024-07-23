using MongoDB.Bson.Serialization.Attributes;
using YourEasyRent.UserState;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace YourEasyRent.Entities
{
    public class Subscriber
    {
        
        public string UserId { get; private set; }
        public string? ChatId { get; private set; }
        public string? Category { get; private set; }
        public string? Brand { get; private set; }
        public string? Name {  get; private set; }

        public decimal? Price { get; private set; }
        public string? Url { get; private set; }
        public Subscriber() { }
        public Subscriber (SubscribersDto subDto)
        {
            UserId = subDto.UserId;
            ChatId = subDto.ChatId;
            Category = subDto.Category;
            Brand = subDto.Brand;
            Name = subDto.Name;//  возможно потом нужно будет добавить цену, чтобы отслежиывать снижение и ссылку на продукт
            Price = subDto.Price;
            Url = subDto.Url;
        }


        public static Subscriber TransferDataToSubscriber(UserSearchState userSearchState, List<string> intermadiateResultList) // переносим одни данные в оъбект Subscriber
        { // а в аргумент передать UserSearchState UserId
            Subscriber subscriber = new Subscriber()
            {
                UserId = userSearchState.UserId,
                ChatId = userSearchState.ChatId,
                Category = userSearchState.Category,
                Brand = intermadiateResultList[0],
                Name = intermadiateResultList[1],
                Price = decimal.Parse(intermadiateResultList[2]),
                Url = intermadiateResultList[3]
            };
            return subscriber;

        }

        public SubscribersDto ToMongoRepresentationSubscriber ()
        {
            SubscribersDto subscribersDto = new SubscribersDto()
            {
                UserId = UserId,
                ChatId = ChatId,
                Category = Category,
                Brand = Brand,
                Name = Name,
                Price = Price,
                Url = Url
            };
            return subscribersDto;
            
        }

    }
}

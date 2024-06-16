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

        public Subscriber() { }
        public Subscriber (SubscribersDto subDto)
        {
            UserId = subDto.UserId;
            ChatId = subDto.ChatId;
            Category = subDto.Category;
            Brand = subDto.Brand;
        }


        public static Subscriber TransferDataToSubscriber(UserSearchState userSearchState) // переносим одни данные в оъбект Subscriber
        { 
            Subscriber subscriber = new Subscriber()
            {
                UserId = userSearchState.UserId,
                ChatId = userSearchState.ChatId,
                Category = userSearchState.Category,
                Brand = userSearchState.Brand,
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
                Brand = Brand
            };
            return subscribersDto;
            
        }

    }
}

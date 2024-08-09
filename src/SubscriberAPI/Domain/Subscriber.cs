using System.Text.Json;
using System.Text.Json.Serialization;

namespace SubscriberAPI.Domain
{
    public class Subscriber
    {
        public string Id { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get;  set; }

        public string ChatId { get; private set; }
        //public string? Category { get; private set; }
        public string? Brand { get;  set; }
        public string? Name { get; private set; }

        public decimal? Price { get; private set; }
        public string? Url { get; private set; }

        public Subscriber() { }
        public Subscriber(SudscriberDto subDto)
        {
            UserId = subDto.UserId;
            ChatId = subDto.ChatId;
            //Category = subDto.Category;
            Brand = subDto.Brand;
            Name = subDto.Name;
            Price = subDto.Price;
            Url = subDto.Url;
        }


        //public static Subscriber TransferDataToSubscriber(UserSearchState userSearchState, List<string> intermadiateResultList)// userSearchStateRequest
        //{ 
        //    Subscriber subscriber = new Subscriber()
        //    {
        //        UserId = userSearchState.UserId,
        //        ChatId = userSearchState.ChatId,
        //        Category = userSearchState.Category,
        //        Brand = intermadiateResultList[0],
        //        Name = intermadiateResultList[1],
        //        Price = decimal.Parse(intermadiateResultList[2]),
        //        Url = intermadiateResultList[3]
        //    };
        //    return subscriber;

        //}

        public SudscriberDto RepresentationSubscriber()
        {
            SudscriberDto subscribersDto = new SudscriberDto()
            {
                UserId = UserId,
                ChatId = ChatId,
               // Category = Category,
                Brand = Brand,
                Name = Name,
                Price = Price,
                Url = Url
            };
            return subscribersDto;

        }

    }
}

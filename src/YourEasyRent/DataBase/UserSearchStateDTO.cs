using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YourEasyRent.Entities;

namespace YourEasyRent.UserState
{

    public class UserSearchStateDTO 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]     
        public string Id { get; set; }


        [BsonRepresentation(BsonType.String)]
        public string UserId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string ChatId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Category { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Brand { get; set; }

        [BsonRepresentation(BsonType.String)]
        public MenuStatus Status { get; set; }

        [BsonRepresentation(BsonType.String)]
        public List<MenuStatus> HistoryOfMenuStatuses { get; set; }

    }


}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace YourEasyRent.Entities
{
    public class SubscribersDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string UserId { get;  set; }
        public string? ChatId { get;  set; }
        public string? Category { get; set; }
        public string? Brand { get; set; }
    }
}

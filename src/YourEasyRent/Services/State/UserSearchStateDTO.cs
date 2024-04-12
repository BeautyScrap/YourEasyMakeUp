using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace YourEasyRent.Services.State
{
    public class UserSearchStateDTO // шаблон с данными, которые должны храниться в бд
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public long UserId { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public long ChatId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Category { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Brand { get; set; }

        [BsonRepresentation(BsonType.String)]
        public MenuStatus Status { get; set; }

    }


}

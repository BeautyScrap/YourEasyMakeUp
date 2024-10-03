using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace YourEasyRent.Entities.ProductForSubscription
{
    public class ProductForSubscriptionDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string? UserId { get; set; }
        public string? ChatId { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Url { get; set; }

    }
}

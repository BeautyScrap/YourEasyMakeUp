using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProductAPI.Domain.ProductForSubscription
{
    public class ProductForSubDto
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string? id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string UserId { get; set; }
        public string Name { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
    }
}

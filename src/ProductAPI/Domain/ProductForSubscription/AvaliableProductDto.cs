using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProductAPI.Domain.ProductForSubscription
{
    public class AvaliableProductDto
    {
        public string Brand { get; set; }
        public string Name { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
        public string Url { get; set; }
    }
}

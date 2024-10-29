using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProductAPI.Domain.Product
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Site Site { get; set; }
        public string SiteId { get; set; }

        public string Brand { get; set; }

        public string Name { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public decimal Price { get; set; }

        public string Category { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
    }
}

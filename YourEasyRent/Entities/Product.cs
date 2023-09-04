
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace YourEasyRent.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]   
        public string? Id { get; set; }

        //[BsonId]
        [BsonRepresentation(BsonType.String)]
        public Site Site { get; set; }
        public string SiteId { get; set; }

        public string Brand { get; set; }

        public string _name;
        public string Name { get;  set; }
        public decimal Price { get; set; }

        public string Category { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
    }
}

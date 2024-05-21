using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YourEasyRent.Entities;

namespace YourEasyRent.UserState
{

    public class BrandAndCaegoryFromSearchStateDTO
    {
        [BsonRepresentation(BsonType.String)]
        public string Category { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Brand { get; set; }
    }
}

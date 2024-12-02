using ProductAPI.Domain.Douglas;
using System.Text.Json.Serialization;

namespace ProductAPI.Contracts.DouglasContract
{
    public class DouglasResponse
    {
        [JsonPropertyName("products")]
        public List<DouglasProduct> Products { get; set; }

        public DouglasResponse()
        {
            Products = new List<DouglasProduct>();
        }
    }
}

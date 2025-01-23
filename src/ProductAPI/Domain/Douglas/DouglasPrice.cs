using Newtonsoft.Json;

namespace ProductAPI.Domain.Douglas
{
    public class DouglasPrice
    {
        [JsonProperty("value")]
        public decimal Value { get; set; }
    }
}
using Newtonsoft.Json;

namespace YourEasyRent.Entities.Douglas
{
    public class DouglasPrice
    {
        [JsonProperty("value")]
        public decimal Value { get; set; }   


    }
}
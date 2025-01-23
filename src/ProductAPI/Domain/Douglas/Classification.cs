using Newtonsoft.Json;

namespace ProductAPI.Domain.Douglas
{
    public class Classification
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
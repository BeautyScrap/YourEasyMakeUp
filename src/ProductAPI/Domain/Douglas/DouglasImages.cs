using Newtonsoft.Json;

namespace ProductAPI.Domain.Douglas
{
    public class DouglasImages
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
using Newtonsoft.Json;

namespace ProductAPI.Domain.Douglas
{
    public class DouglasBrand
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
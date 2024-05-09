using Newtonsoft.Json;

namespace YourEasyRent.Entities.Douglas
{
    public class Classification
    {
        [JsonProperty("name")]
        public string Name { get; set; }    
    }
}
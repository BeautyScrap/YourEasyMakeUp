using System.Text.Json.Serialization;

namespace YourEasyRent.Entities.Douglas
{
    public class DouglasCategory
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }    
    }
}
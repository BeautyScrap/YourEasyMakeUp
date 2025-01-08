using Newtonsoft.Json;

namespace TelegramBotAPI.Contracts
{
    public class FoundBrandResponse
    {
        [JsonProperty("brand")]
        public string Brand { get; set; }
    }
}

using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Collections.Generic;
using YourEasyRent.Entities.Douglas;

namespace TelegramBotAPI.Contracts
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

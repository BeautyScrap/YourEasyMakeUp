using Newtonsoft.Json;

namespace ProductAPI.Domain.Douglas
{
    public class DouglasProduct // проверить как теперь работает этот класс
    {
        [JsonProperty("brand")]
        public DouglasBrand Brand { get; set; }
        public string BaseProductName { get; set; }

        [JsonProperty("priceData")]
        public DouglasPrice Price { get; set; }

        [JsonProperty("classifications")]
        public Classification Classifications { get; set; }
        public string Url { get; set; }

        [JsonProperty("images")]
        public DouglasImages Images { get; set; }

         // было 2 листа
        //[JsonProperty("classifications")]
        //public List<Classification> Classifications { get; set; }

        //[JsonProperty("images")]
        //public List<DouglasImages> Images { get; set; }
    }
}

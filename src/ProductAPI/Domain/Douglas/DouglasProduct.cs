using Newtonsoft.Json;

namespace ProductAPI.Domain.Douglas
{
    public class DouglasProduct
    {
        public string Code { get; set; } // этот параметр вообще можно убрать

        public string Url { get; set; }

        [JsonProperty("images")]
        public List<DouglasImages> Images { get; set; }

        [JsonProperty("classifications")]
        public List<Classification> Classifications { get; set; }

        [JsonProperty("priceData")]
        public DouglasPrice Price { get; set; }

        [JsonProperty("brand")]
        public DouglasBrand Brand { get; set; }
        public string BaseProductName { get; set; }

        public DouglasProduct() { }
    }
}

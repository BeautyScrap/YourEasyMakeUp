using Newtonsoft.Json;

namespace YourEasyRent.Entities.Douglas
{
    public class DouglasProduct // брать названия из Json формата и потом сопоставлять их с названиями из Entities/product (которые универсальны для всех)
    {
        public string Code { get; set; }

        public string Url { get; set; } // было public string Url { get; set; }

        [JsonProperty("images")]
        public List<DouglasImages> Images { get; set; }


        [JsonProperty("classifications")]
        public List<Classification> Classifications { get; set; }

        [JsonProperty("priceData")]
        public DouglasPrice Price { get; set; }


        [JsonProperty("brand")]
        public DouglasBrand Brand { get; set; } // "brand":{"code":"b7867","name":"Charlotte Tilbury" -  мы смотрим на класс brand  и в нем ищем переменную, которая называется name


        public string BaseProductName { get; set; }


      
        public DouglasProduct()
        {
            
        }

    }
}

using Newtonsoft.Json;

namespace YourEasyRent.Entities.Douglas
{
    public class DouglasBrand
    {
        [JsonProperty("name")]
        public string Name { get; set; }  // сделать еще несколько отдельных классов, чтобы смапить названия Json с названиями нашего продукта

    }
}
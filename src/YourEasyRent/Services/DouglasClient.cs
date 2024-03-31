using YourEasyRent.Entities;
using System.Text.Json;
using YourEasyRent.Entities.Douglas;
using static System.Net.WebRequestMethods;

namespace YourEasyRent.Services
{
    public class DouglasClient : IProductsSiteClient
    {
        private readonly HttpClient _httpClient;
        private readonly Dictionary<Section, string> sectionMapping = new()
        {
            [Section.Makeup] = "make-up"
        };
        private readonly JsonSerializerOptions _options;
        private readonly string _baseProductPageUrl = "https://www.douglas.de/de/c/";

        public Site Site => Site.Douglas;

        public DouglasClient(HttpClient client)
        {
            _httpClient = client;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/123.0.0.0 Safari/537.36");
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }


        public async Task<IEnumerable<Product>> FetchFromSectionAndPage(Section section, int pageNumber)
        {
            var url = GetSectionUrl(sectionMapping[section], pageNumber);

            var douglasHTTPResponce = await _httpClient.GetAsync(url);   // код 403

            var douglasRawJsonString = await douglasHTTPResponce.Content.ReadAsStringAsync();

            var douglasProducts = JsonSerializer.Deserialize<DouglasResponse>(douglasRawJsonString, _options); // тут  выдает ошибку

            var products = douglasProducts.Products.Select(p => ToProduct(p));

            return products;
        }

        private string GetSectionUrl(string section, int pageNumber)
        {
            return $"{_baseProductPageUrl}{section}/03?page={pageNumber}";
        }

        private static Product ToProduct(DouglasProduct p)  
        {
            var product = new Product  
            {
                SiteId = p.Code,
                Url = $"https://www.douglas.de{p.Url}",
                Price = p.Price.Value,
                ImageUrl = p.Images.First().Url,
                Brand = p.Brand.Name,
                Name = p.BaseProductName,
                Category = p.Classifications.First().Name,

            };
            return product;  
        }
    }

}

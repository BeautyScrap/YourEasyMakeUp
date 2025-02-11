using ProductAPI.Domain.Douglas;
using ProductAPI.Domain.Product;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProductAPI.Infrastructure.Client
{
    public class DouglasClient //  пока дуглас не использую! только сефора
    {
        private readonly HttpClient _httpClient;
        public Site Site => Site.Douglas;
        private readonly string _baseUrl = $"https://www.douglas.de/de/c/";
        private readonly JsonSerializerOptions _options;
        private readonly Dictionary<Section, string> sectionMapping = new()
        {
            [Section.Makeup] = "make-up"
        };
        public DouglasClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/123.0.0.0 Safari/537.36");
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IEnumerable<Product>> FetchFromSectionAndPage(Section section, int pagenumber)
        {
            var url = GetSectionUrl(sectionMapping[section], pagenumber);
            var httpResponse = await _httpClient.GetAsync(url);
            var jsonString = await httpResponse.Content.ReadAsStringAsync();
            var douglasProducts = JsonSerializer.Deserialize<List<DouglasProduct>>(jsonString, _options);
            var products = douglasProducts.Select(p => Product.CreateProduct(Site.Douglas,p.Brand.Name, p.BaseProductName, p.Price.Value, p.Classifications.Name, p.Url, p.Images.Url));           
            return products;
        }

        public string GetSectionUrl(string section, int pagenumber)
        {
            return $"{_baseUrl}{section}/03?page={pagenumber}"; 
        }

        //private static Product ToProduct(DouglasProduct p)// надо придумать как изменить этот метод, потому что мне он уже не подходит
        //{
        //    var product = new Product
        //    {
        //        SiteId = p.Code,
        //        Url = $"https://www.douglas.de{p.Url}",
        //        Price = p.Price.Value,
        //        ImageUrl = p.Images.First().Url,
        //        Brand = p.Brand.Name,
        //        Name = p.BaseProductName,
        //        Category = p.Classifications.First().Name,
        //    };
        //    return product;
        //}
    }
}

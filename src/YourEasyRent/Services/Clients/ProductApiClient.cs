using System.Net.Http.Headers;
using System.Text.Json;
using TelegramBotAPI.Contracts;

namespace TelegramBotAPI.Services
{
    public class ProductApiClient : IProductApiClient
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        public ProductApiClient(HttpClient client)
        {
            _client = client;
            client.BaseAddress = new Uri("https://localhost:7068/");
            _options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task<List<string>> GetBrandForMenu(string chatId, int limit)
        {
            var httpResponse = await _client.GetAsync($"SearchBrands");
            httpResponse.EnsureSuccessStatusCode();
            var jsonString = await httpResponse.Content.ReadAsStringAsync();
            var brandNames = JsonSerializer.Deserialize<List<FoundBrandResponse>>(jsonString,_options);  
            if (brandNames == null)
            {
                Console.WriteLine("Deserialization resulted in null.");
                throw new Exception("Deserialization failed.");
            }
            var brands = brandNames.Select(x => x.Brand).ToList();
            return brands;            
        }

        public async Task<IEnumerable<string>> GetProductsResultForUser(List<string> listWithResult)
        {
            var searchProductsResultRequest = new SearchProductResultRequest()
            {
                Brand = listWithResult[0],
                Category = listWithResult[1]
            };
            var httpRequest = await _client.PostAsJsonAsync($"SearchProductsResultForUser", searchProductsResultRequest);
            httpRequest.EnsureSuccessStatusCode();
            var jsonString =  await httpRequest.Content.ReadAsStringAsync();
            var productsResult = JsonSerializer.Deserialize<List<FoundProductResultResponse>>(jsonString,_options);
            if (productsResult == null)
            {
                Console.WriteLine("Deserialization resulted in null.");
                throw new Exception("Deserialization failed.");
            }
            var productsString = productsResult.Select(p =>
            $"*{p.Brand}*\n" +
            $"{p.Name}\n" +
            $"{p.Category}\n" +
            $"{p.Price}\n" +
            $"[.]({p.ImageUrl})\n" +
            $"[Ссылка на продукт]({p.Url})");

            return productsString;
        }
    }
}

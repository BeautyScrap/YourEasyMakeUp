using System.Text.Json;

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
            //client.DefaultRequestHeaders.Add("Accept", "application/json"); не знааю надо ли его добавлять

        }

        public async Task<List<string>> GetBrandForMenu(string chatId, int limit)
        {
            var httpResponse = await _client.GetAsync($"/SearchBrands/");
            httpResponse.EnsureSuccessStatusCode();
            var jsonString = await httpResponse.Content.ReadAsStringAsync();
            var brandNames = JsonSerializer.Deserialize<List<string>>(jsonString);
            var brands =  brandNames.ToList();
            return brands;            
        }

    }
}

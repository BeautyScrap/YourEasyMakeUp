using Microsoft.AspNetCore.Http.Connections;
using SubscriberAPI.Contracts.ProductForSubscription;
using SubscriberAPI.Domain;
using System.Text.Json;

namespace SubscriberAPI.Presentanion.Clients
{
    public class ProductApiClient : IProductApiClient
    {
        public readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public ProductApiClient(HttpClient client)
        {
            _client = client;
            client.BaseAddress = new Uri("https://localhost:7068/");
            _options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            client.Timeout = TimeSpan.FromSeconds(300);
        }

        public async Task<List<Subscription>> GetProducts(List<Subscription> subscriptions)
        {
            var request = subscriptions.Select(s => new SearchSubProductRequest()
            {
                UserId = s.UserId,
                Name = s.Name,
                Price = s.Price,
            }).ToList();
            var httpRequest = await _client.PostAsJsonAsync("SearchProductForSubscriber", request);
            httpRequest.EnsureSuccessStatusCode();
            var jsonString = await httpRequest.Content.ReadAsStringAsync();
            var productResult = JsonSerializer.Deserialize<List<FoundSubProductResponse>>(jsonString, _options);
            var products = productResult.Select(p => Subscription.CreateProductforSub(
                p.UserId,
                p.Brand,
                p.Name,
                p.Price,
                p.Url,
                p.ImageUrl)).ToList();
            return products;

        }
    }
}

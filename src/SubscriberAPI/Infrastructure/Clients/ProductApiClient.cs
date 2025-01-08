using Microsoft.AspNetCore.Http.Connections;
using SubscriberAPI.Contracts.ProductForSubscription;
using SubscriberAPI.Domain;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SubscriberAPI.Infrastructure.Clients
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
        }

        public async Task<List<Subscription>> GetProducts(List<Subscription> subscriptions)// AK TO DO  получаем мы не Subscription, а другой объект и уже его пересылаем в телеграм пользаку,
                                                                                           // но это уже 2 действия для одного контроллера, поэтому  можно сделать еще один контроллер, который будет рассылать новые продукты уже в телегу,
                                                                                           // но тогда нужно будет где то хранить новые временные значения
        {
            var request = subscriptions.Select(s => new SearchSubProductRequest()
            {
                UserId = s.UserId,
                Name = s.Name,
                Price = s.Price,
            }).ToList();
            var httpRequest = await _client.PostAsJsonAsync("SearchProductsForSub", request);
            httpRequest.EnsureSuccessStatusCode();
            var jsonString = await httpRequest.Content.ReadAsStringAsync();
            var productResult = JsonSerializer.Deserialize<List<FoundSubProductResponse>>(jsonString, _options);
            var products = productResult.Select(p => Subscription.CreateProductforSub(
                p.UserId,
                p.Brand,
                p.Name,
                p.Price,
                p.Url,
                p.UrlImage)).ToList();
            return products;

            
                
        }
    }
}

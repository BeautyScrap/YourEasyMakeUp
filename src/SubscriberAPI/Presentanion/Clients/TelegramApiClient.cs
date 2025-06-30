using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SubscriberAPI.Contracts.ProductForSubscription;
using SubscriberAPI.Domain;
using System.Text;

namespace SubscriberAPI.Presentanion.Clients
{
    public class TelegramApiClient : ITelegramApiClient
    {
        public readonly HttpClient _client;
        private readonly JsonSerializerSettings _serializerSettings;
        public TelegramApiClient(HttpClient client)
        {
            _client = client;
            client.BaseAddress = new Uri("https://localhost:5001/");
            _serializerSettings = new JsonSerializerSettings 
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };

            client.Timeout = TimeSpan.FromSeconds(300);
        }
        public async Task SendFoundProduct(Subscription subscription)
        {
            try
            {
                var request = new FoundProductForTelegramRequest()
                {
                    UserId = subscription.UserId,
                    Brand = subscription.Brand,
                    Name = subscription.Name,
                    Price = subscription.Price,
                    Url = subscription.Url,
                    ImageUrl = subscription.ImageUrl
                };
                var json = JsonConvert.SerializeObject(request, _serializerSettings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var httpRequest = await _client.PutAsync("UpdateProduct", content);
                httpRequest.EnsureSuccessStatusCode();
                if (!httpRequest.IsSuccessStatusCode)
                {
                    var responseContent = await httpRequest.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Request failed with status {httpRequest.StatusCode}: {responseContent}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

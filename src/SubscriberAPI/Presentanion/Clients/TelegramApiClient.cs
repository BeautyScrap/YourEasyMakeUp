using SubscriberAPI.Contracts.ProductForSubscription;
using SubscriberAPI.Domain;
using System.Text.Json;

namespace SubscriberAPI.Presentanion.Clients
{
    public class TelegramApiClient : ITelegramApiClient
    {
        public readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        public TelegramApiClient(HttpClient client)
        {
            _client = client;
            client.BaseAddress = new Uri("https://localhost:5001/");
            _options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            client.Timeout = TimeSpan.FromSeconds(300);
        }
        public async Task SendFoundProduct(Subscription subscription)// AK TODO вопрос: я какой то ответ от другого сервиса должна получать по идем типо ОК и notFound, если пользак не получил свой продукт?
        {
            var request = new FoundProductForTelegramRequest()
            {
                UserId = subscription.UserId,
                Brand = subscription.Brand,
                Name = subscription.Name,
                Price = subscription.Price,
                Url = subscription.Url,
                UrlImage = subscription.UrlImage
            };
            var httpRequest = await _client.PutAsJsonAsync("UpdateProduct", request);// AK TODO вопрос: -  метод Put  подойдет или нет?
            httpRequest.EnsureSuccessStatusCode();
        }
    }
}

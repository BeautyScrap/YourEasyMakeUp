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
                var httpRequest = await _client.PutAsJsonAsync("UpdateProduct", request, _options);
                httpRequest.EnsureSuccessStatusCode();// AK TODO  LAST UPDATE  вылезает ошибка 404, нужно еще раз протестировать этот запрос
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

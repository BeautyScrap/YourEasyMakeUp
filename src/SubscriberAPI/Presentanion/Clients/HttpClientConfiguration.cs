using Polly;
using Polly.Extensions.Http;
using System.Net;

namespace SubscriberAPI.Presentanion.Clients
{
    public static class HttpClientConfiguration
    {
        public static IHttpClientBuilder ConfigureHttpClient<TClient, TImplementation>(this IServiceCollection services)
        where TClient : class
        where TImplementation : class, TClient
        {
            IHttpClientBuilder httpClientBuilder = services.AddHttpClient<TClient, TImplementation>();
            httpClientBuilder.ConfigurePrimaryHttpMessageHandler(CreateHttpMessageHandler);
            httpClientBuilder.SetHandlerLifetime(Timeout.InfiniteTimeSpan); // Не пересоздаём HttpMessageHandler каждые 2 мин
            httpClientBuilder.AddPolicyHandler(GetRetryPolicy());
            httpClientBuilder.AddPolicyHandler(GetCircuitBreakerPolicy());
            httpClientBuilder.AddPolicyHandler(GetTimeoutPolicy());
            httpClientBuilder.AddPolicyHandler(GetBulkheadPolicy());
            return httpClientBuilder;
        }
        private static HttpMessageHandler CreateHttpMessageHandler()
        {
            return new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2), // Пересоздаём соединение каждые 2 мин
                PooledConnectionIdleTimeout = TimeSpan.FromSeconds(30), // Закрываем неактивные соединения  каждые 30 сек
                MaxConnectionsPerServer = 100 // // Ограничиваем число соединени
            };
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(message => message.StatusCode == HttpStatusCode.RequestTimeout)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))); // Экспоненциальная задержка
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() // Circuit Breaker (Выключатель)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)); // Отключаемся на 30 сек после 5 ошибок подряд
        }

        private static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
        {
            return Policy
                .TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(50)); // Ждём максимум 10 сек
        }

        private static IAsyncPolicy<HttpResponseMessage> GetBulkheadPolicy()
        {
            return Policy
                .BulkheadAsync<HttpResponseMessage>(maxParallelization: 20, maxQueuingActions: 30); // Ограничиваем количество одновременных запросов, чтобы не перегружать сервис
        }
    }
}

using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Microsoft.AspNetCore.Http.HttpResults;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Entities.ProductForSubscription;
using YourEasyRent.TelegramMenu;

namespace YourEasyRent.Services
{
    public class ProductForSubscriptionService
    {
        private readonly ILogger<ProductForSubscriptionService> _logger;
        private readonly IProductRepository _productRepository;
        private readonly ITelegramSender _telegramSender;
        private readonly IRabbitMessageProducer _messageProducer;

        public ProductForSubscriptionService(ILogger<ProductForSubscriptionService> logger, IProductRepository repository, ITelegramSender telegramSender, IRabbitMessageProducer messageProducer)
        {
            _logger = logger;
            _productRepository = repository;
            _telegramSender = telegramSender;
            _messageProducer = messageProducer;
        }
        public async Task ProductHandler(List<ProductForSubscription> products)
        {
            var listWithFoundProducts = new List<ProductForSubscription>();  
            foreach (var product in products)
            {
                 // var userId= product.UserId; -   не преобразовывая его в дто?
                var productDto = product.ToDto(); 
                var userId = productDto.UserId;
                var chatId = productDto.ChatId;

                var resultProductDto = await _productRepository.GetProductForOneSubscriber(productDto);
                if (resultProductDto == null)
                {
                    continue;
                } 
                ProductForSubscription productForSubscription = ProductForSubscription.GlueResultOfSearch(userId, chatId, resultProductDto);
                await _telegramSender.SendSubscriberProduct(chatId, productForSubscription);
                listWithFoundProducts.Add(productForSubscription);
            }
            _messageProducer.SendMessagAboutSubscriber(listWithFoundProducts);// - AK TODO  нужна наверно правильная очередь с отределенным назыанием или нет, достаточно знать в какой микросервис послать?

        }
    }
}


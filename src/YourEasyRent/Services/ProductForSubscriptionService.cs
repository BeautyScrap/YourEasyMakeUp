﻿using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Microsoft.AspNetCore.Http.HttpResults;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Entities.ProductForSubscription;
using YourEasyRent.TelegramMenu;

namespace YourEasyRent.Services
{
    public class ProductForSubscriptionService: IProductForSubscriptionService
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
        public async Task<List<ProductForSubscription>> ProductHandler(List<ProductForSubscription> products)
        {
            var listWithFoundProducts = new List<ProductForSubscription>();  
            foreach (var product in products)
            {
                var productDto = product.ToDto(); 
                var userId = productDto.UserId;
                var chatId = productDto.ChatId;

                var resultProductDto = await _productRepository.GetProductForOneSubscriber(productDto);
                if (resultProductDto == null)
                {
                    continue; 
                } 
                ProductForSubscription productForSubscription = ProductForSubscription.GlueResultOfSearch(userId, chatId, resultProductDto);
               // await _telegramSender.SendSubscriberProduct(chatId, productForSubscription); // AK TODO  правильно, что я передаю сообщение пользаку из этого сервиса, наверно нет? Тогда я должна передавать новую ифу с новой ценой и ссылкой на продукт через userId  отбатно в SubscriberAPI, а в самом контроллере SubscriberAPI принять эту инфу и образобать информацию или для этого нужен другой контроллер?
                listWithFoundProducts.Add(productForSubscription);
            }
            return listWithFoundProducts;
        }
    }
}


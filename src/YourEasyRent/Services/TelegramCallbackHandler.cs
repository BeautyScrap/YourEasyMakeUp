using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using YourEasyRent.Entities;
using System.Data;
using YourEasyRent.Services.Buttons;
using Microsoft.AspNetCore.Http.Connections;
using System.Security.Cryptography.Xml;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.UserState;
using YourEasyRent.TelegramMenu;
using System.Collections.Generic;
using YourEasyRent.DataBase;
using YourEasyRent.Entities.ProductForSubscription;

namespace YourEasyRent.Services
{
    public class TelegramCallbackHandler : ITelegramCallbackHandler
    {

        private readonly IUserStateRepository _userStateRepository;
        private readonly ITelegramSender _telegramSender;
        private readonly ILogger<TelegramCallbackHandler> _logger;
        private readonly IProductRepository _productRepository;
        private readonly ISubscribersRepository _subscribersRepository;
        private readonly IRabbitMessageProducer _rabbitMessageProducer;



        public TelegramCallbackHandler
            (
            ILogger<TelegramCallbackHandler> logger,
            IUserStateRepository userStateRepository,
            ITelegramSender telegramSender,
            ISubscribersRepository subscribersRepository,
            IProductRepository productRepository,
            IRabbitMessageProducer rabbitMessageProducer
           
            )
        {
            _logger = logger;
            _userStateRepository = userStateRepository;
            _telegramSender = telegramSender;
            _subscribersRepository = subscribersRepository;
            _productRepository = productRepository;
            _rabbitMessageProducer = rabbitMessageProducer ?? throw new ArgumentNullException(nameof(rabbitMessageProducer));
        }

        public async Task HandleUpdateAsync(TgButtonCallback tgButtonCallback)
        {

            var userId = tgButtonCallback.GetUserId();
            var chatId = tgButtonCallback.GetChatId();

            if (tgButtonCallback.IsStart)
            {
                var userSearchState = UserSearchState.CreateNewUserSearchState(userId);
                userSearchState.SetChatId(chatId);
                userSearchState.AddStatusToHistory(MenuStatus.MainMenu);
                await _userStateRepository.CreateAsync(userSearchState);
                await _telegramSender.SendMainMenu(chatId);
                return;
            };

            if (tgButtonCallback.IsValidMessage)
            {
                if (tgButtonCallback.IsValueMenuMessage)
                {
                    UserSearchState userSearchState = await _userStateRepository.GetForUser(userId);

                    if (tgButtonCallback.IsBrandMenu)
                    {
                        userSearchState.AddStatusToHistory(MenuStatus.BrandMenu);
                        await _userStateRepository.UpdateAsync(userSearchState);
                        await _telegramSender.SendBrandMenu(chatId);
                        return;
                    }

                    if (tgButtonCallback.IsCategoryMenu)
                    {                        
                        userSearchState.AddStatusToHistory(MenuStatus.CategoryMenu);
                        await _userStateRepository.UpdateAsync(userSearchState);
                        await _telegramSender.SendCategoryMenu(chatId);
                        return;
                    }

                }

                if (tgButtonCallback.IsValueProductButton)
                {
                    UserSearchState userSearchState = await _userStateRepository.GetForUser(userId);

                    if (tgButtonCallback.IsProductBrand)
                    {
                        userSearchState.AddStatusToHistory(MenuStatus.BrandChosen);
                        var brand = tgButtonCallback.GetProductButton();
                        userSearchState.SetBrand(brand);
                        await _userStateRepository.UpdateAsync(userSearchState);

                        if (userSearchState.IsFinished)
                        {
                            var tupleWithResult = await _userStateRepository.GetFilteredProductsForSearch(userId); // метод, который вернет резултат для переменной
                            var listWithResult = new List<string> { tupleWithResult.Brand, tupleWithResult.Category }.ToList();
                            await _telegramSender.SendResults(chatId, listWithResult);
                            await _telegramSender.SendMenuAfterResult(chatId);  
                            return;
                        }
                        await _telegramSender.SendCategoryMenu(chatId); 
                        return;
                    };

                    if (tgButtonCallback.IsProductCategory)
                    {
                        userSearchState.AddStatusToHistory(MenuStatus.CategoryChosen);//  можно этот метод перенести внуть метода GetProductButton
                        var category = tgButtonCallback.GetProductButton();
                        userSearchState.SetCategory(category);
                        await _userStateRepository.UpdateAsync(userSearchState);

                        if (userSearchState.IsFinished)
                        {
                            var tupleWithResult = await _userStateRepository.GetFilteredProductsForSearch(userId); 
                            var listWithResult = new List<string> { tupleWithResult.Brand, tupleWithResult.Category }.ToList();
                            await _telegramSender.SendResults(chatId, listWithResult);
                            await _telegramSender.SendMenuAfterResult(chatId);
                            return;
                        }
                        await _telegramSender.SendCategoryMenu(chatId);
                        return;
                    };
                }
                if (tgButtonCallback.IsSubscribeToProduct)
                { 
                    UserSearchState userSearchState = await _userStateRepository.GetForUser(userId);
                    userSearchState.AddStatusToHistory(MenuStatus.SubscribedToTheProduct);
                    await _userStateRepository.UpdateAsync(userSearchState);
                    
                    var tupleWithResultFromUSR = await _userStateRepository.GetFilteredProductsForSearch(userId); 
                    var listWithResult = new List<string> { tupleWithResultFromUSR.Brand, tupleWithResultFromUSR.Category }.ToList();

                    var intermediateResult = await GetFilteredProductsFromProductRepository(listWithResult);
                    var intermadiateResultList = new List<string>
                    {
                        intermediateResult.Brand,
                        intermediateResult.Name,
                        intermediateResult.Price.ToString(),
                        intermediateResult.Url
                    }.ToList();

                    var subscriber = ProductForSubscription.TransferDataToSubscriber(userSearchState, intermadiateResultList);
                    //await _subscribersRepository.CreateSubscriberAsync(subscriber); //  потом удалить этот метод, тк мы не сохраняем подписчика в этом приложении( удалить еще репозиторий для него)
                    _rabbitMessageProducer.SendMessagAboutSubscriber(subscriber); // отправляю сообщение в другой сервис
                    await _telegramSender.SendConfirmOfSubscriprion(chatId);
                    return;
                }

            }
        }

        public async Task<(string? Brand, string? Name, decimal? Price, string? Url)> GetFilteredProductsFromProductRepository(List<string> listWithResult)
        {
            var products = await _productRepository.GetProductsByBrandAndCategory(listWithResult);
            {
                var firstProduct = products.FirstOrDefault();
                if (firstProduct == null)
                {
                    return (null, null, 0, null);
                }
                return (firstProduct.Brand, firstProduct.Name, firstProduct.Price, firstProduct.Url);
            }
        }
    }
}
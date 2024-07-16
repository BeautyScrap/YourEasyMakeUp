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

namespace YourEasyRent.Services
{
    public class TelegramCallbackHandler : ITelegramCallbackHandler
    {

        private readonly IUserStateRepository _userStateRepository;
        private readonly ITelegramSender _telegramSender;
        private readonly ILogger<TelegramCallbackHandler> _logger;
        private readonly ISubscribersRepository _subscribersRepository;


        public TelegramCallbackHandler
            (
            ILogger<TelegramCallbackHandler> logger,
            IUserStateRepository userStateRepository,
            ITelegramSender telegramSender,
            ISubscribersRepository subscribersRepository

            )
        {
            _logger = logger;
            _userStateRepository = userStateRepository;
            _telegramSender = telegramSender;
            _subscribersRepository = subscribersRepository;
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

                            // Сюда еще можно вставить метод  после получения результата  SendMenuAfterResult
                            
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
                    var subscriber = Subscriber.TransferDataToSubscriber(userSearchState);
                    await _subscribersRepository.CreateSubscriberAsync(subscriber);

                    var tupleWithResult = await _userStateRepository.GetFilteredProductsForSearch(userId); 
                    var listWithResult = new List<string> { tupleWithResult.Brand, tupleWithResult.Category }.ToList();

                    await _telegramSender.SendConfirmOfSubscriprion(chatId);
                    return;

                }

            }
        }
    }
}
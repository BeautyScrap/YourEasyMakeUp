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
        private readonly ITelegramBotClient _botClient;
        private readonly IProductRepository _productRepository;
        private readonly IUserStateRepository _userStateRepository;
        private readonly ITelegramSender _telegramSender;
        private readonly ILogger<TelegramCallbackHandler> _logger;


        public TelegramCallbackHandler
            (
            ITelegramBotClient botClient,
            IProductRepository productRepository,
            ILogger<TelegramCallbackHandler> logger,
            IUserStateRepository userStateRepository,
            ITelegramSender telegramSender

            )
        {
            _botClient = botClient;
            _productRepository = productRepository;
            _logger = logger;
            _userStateRepository = userStateRepository;
            _telegramSender = telegramSender;

        }

        public async Task HandleUpdateAsync(TgButtonCallback tgButtonCallback)
        {

            var userId = tgButtonCallback.GetUserId();
            var chatId = tgButtonCallback.GetChatId();

            if (tgButtonCallback.IsStart)
            {
                var userSearchState = UserSearchState.CreateNewUserSearchState(userId);

                userSearchState.SetChatId(chatId);
                MenuStatus status = MenuStatus.MainMenu;
                userSearchState.AddStatusToHistory(status);
                userSearchState.ToMongoRepresintation();
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
                        MenuStatus status = MenuStatus.BrandMenu;
                        userSearchState.AddStatusToHistory(status);
                        userSearchState.ToMongoRepresintation();
                        await _userStateRepository.UpdateAsync(userSearchState);
                        await _telegramSender.SendBrandMenu(chatId);
                        return;
                    }

                    if (tgButtonCallback.IsCategoryMenu)
                    {
                        MenuStatus status = MenuStatus.CategoryMenu;
                        userSearchState.AddStatusToHistory(status);
                        userSearchState.ToMongoRepresintation();
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
                        MenuStatus status = MenuStatus.BrandChosen;
                        userSearchState.AddStatusToHistory(status);

                        var brand = tgButtonCallback.GetProductButton();
                        userSearchState.SetBrand(brand);
                        await _userStateRepository.UpdateAsync(userSearchState);

                        if (userSearchState.IsFinished)
                        {
                            var tupleWithResult = await _userStateRepository.GetFilteredProductsForSearch(userId); // метод, который вернет резултат для переменной
                            var listWithResult = new List<string> { tupleWithResult.Brand, tupleWithResult.Category }.ToList();
                            await _telegramSender.SendResults(chatId, listWithResult);
                            return;
                        }
                        await _telegramSender.SendCategoryMenu(chatId); 
                        return;
                    };
                    if (tgButtonCallback.IsProductBrand)
                    {
                        MenuStatus status = MenuStatus.CategoryChosen;
                        userSearchState.AddStatusToHistory(status);

                        var category = tgButtonCallback.GetProductButton();
                        userSearchState.SetCategory(category);
                        await _userStateRepository.UpdateAsync(userSearchState);

                        if (userSearchState.IsFinished)
                        {
                            var tupleWithResult = await _userStateRepository.GetFilteredProductsForSearch(userId); 
                            var listWithResult = new List<string> { tupleWithResult.Brand, tupleWithResult.Category }.ToList();
                            await _telegramSender.SendResults(chatId, listWithResult);
                            return;
                        }
                        //  проверка, что все поля заполнены, если все заполенно то вызвать метод SendResult, 
                        // если нет, то вызов другого меню
                        await _telegramSender.SendCategoryMenu(chatId);
                        return;
                    };
                }
            }
        }
    }
}
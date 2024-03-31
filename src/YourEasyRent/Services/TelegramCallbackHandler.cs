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

using YourEasyRent.Services.State;

namespace YourEasyRent.Services
{
    public class TelegramCallbackHandler : ITelegramCallbackHandler
    {
        private readonly ITelegramBotClient _botClient;
        private ITelegramActionsHandler _actionsHandler;
        private Dictionary<string, IButtonHandler> _buttonHandlers;
        private Dictionary<long, BotState> _userResponsesToChat = new();
        private readonly ILogger<TelegramCallbackHandler> _logger;
        private readonly IProductRepository _productRepository;
        private MenuStatus _currentMenuStatus = MenuStatus.Started;
        private readonly IUserStateManager _userStateManager = new UserStateManager();

        public TelegramCallbackHandler
            (
            ITelegramBotClient botClient,
            ITelegramActionsHandler actionsHandler,
            IProductRepository productRepository,
            IUserStateManager userStateManager,
            ILogger<TelegramCallbackHandler> logger
            )
        {
            _botClient = botClient;
            _actionsHandler = actionsHandler;
            _productRepository = productRepository;
            _buttonHandlers = new Dictionary<string, IButtonHandler>()
            {
                {"MainMenu" , new MainMenuButtonHandler(_botClient) },
                {"BrandMenu", new BrandButtonHandler(_botClient, _productRepository) },
                {"CategoryMenu", new CategoryButtonHandler(_botClient)  },
                {"ReturnToMainMenu", new ReturnToMMButtonHandler(_botClient) }
            };
            _logger = logger;
            _userStateManager = userStateManager;
        }

        public async Task HandleUpdateAsync(Update update)
        {
            var messageText = update.Message?.Text;
            var firstName = update.Message?.From.FirstName;
            var status = _currentMenuStatus.ToString();
            var buttonName = update.CallbackQuery?.Data;
            _userStateManager.AddStatusToList(status);

            if (messageText != null && messageText.Contains("/start") || status == "Started" || buttonName == "StartNewSearch")
            {
                _userStateManager.ReturnToMainMenu();
                var startedForUserId = update.Message.From.Id;
                _userResponsesToChat[startedForUserId] = new BotState() { ChatId = update.Message.Chat.Id };
                _currentMenuStatus = MenuStatus.MainMenu;
                var mainMenuHandler = _buttonHandlers["MainMenu"];
                await mainMenuHandler.SendMenuToTelegramHandle(startedForUserId);
                _logger.LogInformation(messageText);
                return;
            }
            if (update.Type != UpdateType.CallbackQuery)
            {
                throw new Exception("The user did not send a message");
            }

            var callbackId = update.CallbackQuery.Id;
            var userResponse = update.CallbackQuery?.Data;
            var userId = update.CallbackQuery!.From.Id;
            var chatId = GetChatIdOrDefalt(userId);

            if (buttonName == "Back")
            {
                _userStateManager.GoBackToOneStep(status);
                var lastMenu = _userStateManager.MethodBackOnOneStep().ToString();
                var lastMenuHandler = _buttonHandlers[lastMenu];
                await lastMenuHandler.SendMenuToTelegramHandle(chatId);
                return;

            }

            if (buttonName == "BrandMenu")
            {
                _currentMenuStatus = MenuStatus.BrandMenu;
                var handler = _buttonHandlers[buttonName];
                await handler.SendMenuToTelegramHandle(chatId);
                return;
            }

            if (buttonName == "CategoryMenu")
            {
                _currentMenuStatus = MenuStatus.CategoryMenu;
                var handler = _buttonHandlers[buttonName];
                await handler.SendMenuToTelegramHandle(chatId);
                return;
            }

            var botState = _userResponsesToChat[userId];

            if (buttonName.StartsWith("Brand_"))
            {
                botState.ChatId = update.CallbackQuery!.From.Id;
                botState.Brand = buttonName.Replace("Brand_", "");
                _currentMenuStatus = MenuStatus.BrandChosen;
                _userStateManager.SetBrand(MenuStatus.BrandChosen);
                string resultOfMenu = _userStateManager.CheckStatusInList("BrandChosen");

                if (resultOfMenu != "ReadyToResult")
                {
                    await _buttonHandlers[resultOfMenu].SendMenuToTelegramHandle(chatId);
                    return;
                }

                botState.PropertiesAreFilled();
                var result = SendAllResult(chatId, botState);
                _logger.LogInformation($"2. Received a  button'{buttonName}' userID {userId} and user First Name {firstName} and chatID {chatId}   и callbackId {callbackId}, userResponse {userResponse} ");

                await Task.Delay(3000);

                var handler = _buttonHandlers["ReturnToMainMenu"];
                await handler.SendMenuToTelegramHandle(chatId);
                return;
            }

            if (buttonName.StartsWith("Category_")) // для брендов из БД как мы можем понять что это именно название какого-то бренда, а не кнопка Back&&
            {
                botState.ChatId = update.CallbackQuery!.From.Id;
                botState.Category = buttonName.Replace("Category_", "");
                _currentMenuStatus = MenuStatus.CategoryChosen;
                _userStateManager.SetCategory(MenuStatus.CategoryChosen);
                string resultOfMenu = _userStateManager.CheckStatusInList("CategoryChosen");

                if (resultOfMenu != "ReadyToResult")
                {

                    await _buttonHandlers[resultOfMenu].SendMenuToTelegramHandle(chatId);
                    return;
                }
                botState.PropertiesAreFilled();
                var result = SendAllResult(chatId, botState);
                _logger.LogInformation($"3. Received a  button'{buttonName}' userID {userId} and user First Name {firstName} and chatID {chatId}   и callbackId {callbackId}, userResponse {userResponse} ");
                await Task.Delay(3000);
                var handler = _buttonHandlers["ReturnToMainMenu"];
                await handler.SendMenuToTelegramHandle(chatId);
                return;
            }
        }
        private async Task<IEnumerable<string>> SendAllResult(long chatId, BotState botState)
        {
            string brand = botState.Brand;
            string category = botState.Category;
            var products = await _actionsHandler.GetFilteredProductsMessage(brand, category);
            foreach (var product in products)
            {
                await _botClient.SendTextMessageAsync(chatId, product, parseMode: ParseMode.Markdown);
            }
            return products;
        }
        private long GetChatIdOrDefalt(long userId)
        {
            if (!_userResponsesToChat.ContainsKey(userId))
            {
                return userId;
            }
            var botState = _userResponsesToChat[userId];
            return botState.ChatId;
        }
    }
}
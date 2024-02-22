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

namespace YourEasyRent.Services
{
    public class TelegramCallbackHandler : ITelegramCallbackHandler
    {
        private readonly ITelegramBotClient _botClient;
        private ITelegramActionsHandler _actionsHandler;
        private Dictionary<string, IButtonHandler> _buttonHandlers;
        private Dictionary<long, BotState> _userResponsesToChat = new();
        private readonly ILogger<TelegramCallbackHandler> _logger;
        // сделать еше один словать которыцй будет содержать  userId (long)  и связанный с ним словать  ключ - бренд и категория, значения - Мейбелин и тушь

        public TelegramCallbackHandler(ITelegramBotClient botClient, ITelegramActionsHandler actionsHandler,ILogger<TelegramCallbackHandler> logger)
        {
            _botClient = botClient;
            _actionsHandler = actionsHandler;
            _buttonHandlers = new Dictionary<string, IButtonHandler>()
            {
                { "MainMenu", new MainMenuButtonHandler(_botClient) },
                { "BrandMenu", new BrandButtonHandler(_botClient) },
                { "CategoryMenu", new CategoryButtonHandler(_botClient)  }
            };
               _logger = logger;    
                        //{"ReturnToMenu", new ReturnButtonHandler() },
            //{"Back", new BackReturnHandler() }

        }

        public async Task HandleUpdateAsync(Update update)
        {
            var messageText = update.Message?.Text;
            var firstName = update.Message?.From.FirstName;

            if (messageText != null && messageText.Contains("/start"))
            {
                var startedForUserId = update.Message.From.Id;
                _userResponsesToChat[startedForUserId] = new BotState() { ChatId = update.Message.Chat.Id };

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

            var buttonName = update.CallbackQuery.Data;

            if (buttonName == "BrandMenu" || buttonName == "CategoryMenu")
            {
                var handler = _buttonHandlers[buttonName];// если выбирать какую-то конкретную категорию или бренд, но выдается ошибка, тк в словаре _buttonHandlers нет этих значений
                await handler.SendMenuToTelegramHandle(chatId);
                return;
            }

            _logger.LogInformation($"4. Received a  button'{buttonName}' userID {userId} and user First Name {firstName} and chatID {chatId}   и callbackId {callbackId}, userResponse {userResponse} ");
           // Console.WriteLine($"4. Received a  button'{buttonName}' userID {userId} and user First Name {firstName} and chatID {chatId}   и callbackId {callbackId}, userResponse {userResponse} ");

            var botState = _userResponsesToChat[userId];
            bool result = false;
          
            //var result = botState.PropertiesAreFilled();
            //if (result == true) 
            //{
            //    string brand = botState.Brand;
            //    string category = botState.Category;
            //    await _actionsHandler.GetFilteredProductsMessage(brand, category);
            //    return;

            //}

            if (buttonName.StartsWith("Brand_"))
            {
                botState.ChatId = update.CallbackQuery!.From.Id; // или ChatID?
                botState.Brand = buttonName.Replace("Brand_", "");
                var categoryMenuHandler = _buttonHandlers["CategoryMenu"];
                await categoryMenuHandler.SendMenuToTelegramHandle(chatId);
                botState.Status = BotState.MenuStatus.BrandChosen;
                return;
            }

            if (buttonName.StartsWith("Category_"))
            {
                botState.ChatId = update.CallbackQuery!.From.Id; // или ChatID?
                botState.Category =  buttonName.Replace("Category_", "");
                //var categoryMenuHandler = _buttonHandlers["CategoryMenu"];
                // await categoryMenuHandler.SendMenuToTelegramHandle(chatId);
                botState.Status = BotState.MenuStatus.CategoryChosen;

                result = botState.PropertiesAreFilled();
                if (result == true)
                {
                    string brand = botState.Brand;
                    string category = botState.Category;
                    var products  = await _actionsHandler.GetFilteredProductsMessage(brand, category);
                    foreach ( var product in products )
                    {
                        await _botClient.SendTextMessageAsync(chatId, product, parseMode: ParseMode.Markdown);
                    }
                    _logger.LogInformation($"4. Received a  button'{buttonName}' userID {userId} and user First Name {firstName} and chatID {chatId}   и callbackId {callbackId}, userResponse {userResponse} ");
                    return;
                }

            }

        }

        private long GetChatIdOrDefalt(long userId) 
        {
            if(!_userResponsesToChat.ContainsKey(userId))
            {
                return userId;
            }
            var botState = _userResponsesToChat[userId];
            return botState.ChatId;
        }



    }
}
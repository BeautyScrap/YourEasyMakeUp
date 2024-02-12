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
        // сделать еше один словать которыцй будет содержать  userId (long)  и связанный с ним словать  ключ - бренд и категория, значения - Мейбелин и тушь

        private string _currentBrand = "";
        private string _currentCategory = "";


        public TelegramCallbackHandler(ITelegramBotClient botClient, ITelegramActionsHandler actionsHandler)
        {
            _botClient = botClient;
            _actionsHandler = actionsHandler;
            _buttonHandlers = new Dictionary<string, IButtonHandler>()
            {
                { "MainMenu", new MainMenuButtonHandler(_botClient) },
                { "BrandMenu", new BrandButtonHandler(_botClient) },
                { "CategoryMenu", new CategoryButtonHandler(_botClient)  }
            }; 
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

            Console.WriteLine($"4. Received a  button'{buttonName}' userID {userId} and user First Name {firstName} and chatID {chatId}   и callbackId {callbackId}, userResponse {userResponse} ");

            var botState = _userResponsesToChat[userId];

            if (buttonName.StartsWith("Brand_"))
            {
                botState.ChatId = update.CallbackQuery!.From.Id; // или ChatID?
                botState.Brand = buttonName.Replace("Brand_", "");
                botState.Status = MenuStatus.BrandChosen;
                var categoryMenuHandler = _buttonHandlers["CategoryMenu"];
                await categoryMenuHandler.SendMenuToTelegramHandle(chatId);
                return;

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
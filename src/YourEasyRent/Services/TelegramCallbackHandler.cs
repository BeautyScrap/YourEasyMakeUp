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
using static YourEasyRent.Services.State.MenuStatus;
using YourEasyRent.Services.State;

namespace YourEasyRent.Services
{
    public class TelegramCallbackHandler : ITelegramCallbackHandler
    {
        private readonly ITelegramBotClient _botClient;
        private ITelegramActionsHandler _actionsHandler;
        private Dictionary<string, IButtonHandler> _buttonHandlers;//  может быть сюда вместо строки засунуть menuStatus??
        private Dictionary<long, BotState> _userResponsesToChat = new();
        private readonly ILogger<TelegramCallbackHandler> _logger;
        private readonly IProductRepository _productRepository;
        private MenuStatus _currentMenuStatus = Started;
        private readonly UserStateManager _userStateManager = new UserStateManager(); 
        public TelegramCallbackHandler(ITelegramBotClient botClient, ITelegramActionsHandler actionsHandler,IProductRepository productRepository,UserStateManager userStateManager,MenuStatus menuStatus,ILogger<TelegramCallbackHandler> logger)
        {
            _botClient = botClient;
            _actionsHandler = actionsHandler;

            _productRepository = productRepository;
            _buttonHandlers = new Dictionary<string, IButtonHandler>()
            {
                {"MainMenu" , new MainMenuButtonHandler(_botClient) },
                {"BrandMenu", new BrandButtonHandler(_botClient, _productRepository) },
                {"CategoryMenu", new CategoryButtonHandler(_botClient)  }
            };
            _logger = logger;    
            _userStateManager = userStateManager;  
            _currentMenuStatus = menuStatus;    
                        //{"ReturnToMenu", new ReturnButtonHandler() },
            //{"Back", new BackReturnHandler() }
        }

        public async Task HandleUpdateAsync(Update update)
        {
            var messageText = update.Message?.Text;
            var firstName = update.Message?.From.FirstName;
            var status = _currentMenuStatus.ToString();// в самом начале я  сохраняю актульный статус и добавляю его в list со стоатусами, чтыб дальше выбрать следующий шаг
            _userStateManager.AddStatusToList(status);

            if (messageText != null && messageText.Contains("/start") || status == "Started")// чтобы можно было вернуться назад в главное меню, после одного цикла всей программы
            {
                var startedForUserId = update.Message.From.Id;
                _userResponsesToChat[startedForUserId] = new BotState() { ChatId = update.Message.Chat.Id };
                _currentMenuStatus = MainMenu;
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

            if(buttonName == "Back")
            {
                _userStateManager.BackOnOneStep();
            }

            if (buttonName == "BrandMenu") 
            {
                _currentMenuStatus = BrandMenu;
                var handler = _buttonHandlers[buttonName];
                await handler.SendMenuToTelegramHandle(chatId);
                return;
            }

            if (buttonName == "CategoryMenu") 
            {
                _currentMenuStatus = CategoryMenu;
                var handler = _buttonHandlers[buttonName];
                await handler.SendMenuToTelegramHandle(chatId);
                return;
            }
            _logger.LogInformation($"Received a  button'{buttonName}' userID {userId} and user First Name {firstName} and chatID {chatId}   и callbackId {callbackId}, userResponse {userResponse} ");
          
            var botState = _userResponsesToChat[userId];
            //bool result = false;
         
            if (buttonName.StartsWith("Brand_")) 
            { 
                botState.ChatId = update.CallbackQuery!.From.Id;
                botState.Brand = buttonName.Replace("Brand_", "");
                _currentMenuStatus = BrandChosen;
                _userStateManager.BrandChosen(BrandChosen);
                string resultOfMenu = _userStateManager.CheckStatusInList("BrandChosen");
                
                if (resultOfMenu != "ReadyToResult")
                {
                    await _buttonHandlers[buttonName].SendMenuToTelegramHandle(chatId);
                    return;
                }

                botState.PropertiesAreFilled();
                string brand = botState.Brand;
                string category = botState.Category;
                var products = await _actionsHandler.GetFilteredProductsMessage(brand, category);
                foreach (var product in products)
                {
                    await _botClient.SendTextMessageAsync(chatId, product, parseMode: ParseMode.Markdown);
                }
                _logger.LogInformation($" Received a  button'{buttonName}' userID {userId} and user First Name {firstName} and chatID {chatId}   и callbackId {callbackId}, userResponse {userResponse} ");
                _userStateManager.ReturnToMainMenu();   
                return;
            }

            if (buttonName.StartsWith("Category_")) // для брендов из БД как мы можем понять что это именно название какого-то бренда, а не кнопка Back&&
            { //  выставляем  метод  void BrandChosen(); чтобы пометить, чтоб бренд дейстительно выбран и менюСтатут изменяется на BrandChosen 
                botState.ChatId = update.CallbackQuery!.From.Id;
                botState.Category = buttonName.Replace("Category_", "");
                _currentMenuStatus = CategoryChosen;
                _userStateManager.CategoryChosen(CategoryChosen);
                string resultOfMenu = _userStateManager.CheckStatusInList("CategoryChosen");// // !! и  на этом этапе я уже знаю, что статут BrandChosen записан в лист и после этого телеграм должен сходить в CheckStatusInLis и вызвать мне другое о

                if (resultOfMenu != "ReadyToResult")
                {

                    await _buttonHandlers[buttonName].SendMenuToTelegramHandle(chatId);
                    return;
                }

                botState.PropertiesAreFilled();//  как мне вынести проверку этого метода и формирование ответа отсюда?
                                               //  чтобы не дублировать этот кусок кода везде

                var result  = SendAllResult(chatId, botState);
                
                _logger.LogInformation($" Received a  button'{buttonName}' userID {userId} and user First Name {firstName} and chatID {chatId}   и callbackId {callbackId}, userResponse {userResponse} ");
                _userStateManager.ReturnToMainMenu();//  после получения результатов возможно где то здесь нужно сделать кнопку ReturnToMenu(StartNewSearch), которая перекинет нас обратно в главное меню как то
                //  скинуть предыдущие статусы и результаты, оставив только chatID
                return;
                

            }


        }

        private async Task SendAllResult(long chatId, BotState botState)
        {
            string brand = botState.Brand;
            string category = botState.Category;
            var products = await _actionsHandler.GetFilteredProductsMessage(brand, category);
            foreach (var product in products)
            {
                await _botClient.SendTextMessageAsync(chatId, product, parseMode: ParseMode.Markdown);
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
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
using YourEasyRent.Services.Keyboard;

namespace YourEasyRent.Services
{
    public class TelegramCallbackHandler : ITelegramCallbackHandler
    {
        private readonly ITelegramBotClient _botClient;
        private ITelegramActionsHandler _actionsHandler;
        private readonly IProductRepository _productRepository;
        private readonly IUserStateManagerRepository _userStateRepository;
        private readonly ITelegramSender _telegramSender;
        private readonly ILogger<TelegramCallbackHandler> _logger;
        private readonly IUserSearchState _userSearchState;
        private readonly ITgButtonCallback _tgButtonCallback;

        //
       // private readonly IUserSearchStateFactory _factory;
        // Factory
        //public class UserSearchStateFactory: IUserSearchStateFactory // отдельный класс и интерфейс для фабрики  по созданию новых UserSearchState
        //{
        //    public IUserSearchState CreateNew()
        //    {
        //        return new UserSearchState();
        //    }
        //}

        //public interface IUserSearchStateFactory
        //{
        //    IUserSearchState CreateNew();
        //}

        public TelegramCallbackHandler
            (
            ITelegramBotClient botClient,
            ITelegramActionsHandler actionsHandler,
            IProductRepository productRepository,
            ILogger<TelegramCallbackHandler> logger,
            IUserStateManagerRepository userStateRepository,
            ITelegramSender telegramSender,
            //IUserSearchStateFactory factory,
            ITgButtonCallback tgButtonCallback,
            IUserSearchState userSearchState
            )
        {
            _botClient = botClient;
            _actionsHandler = actionsHandler;
            _productRepository = productRepository;
            _logger = logger;
            _userStateRepository = userStateRepository;
            _telegramSender = telegramSender;
            _tgButtonCallback = tgButtonCallback;
            _userSearchState = userSearchState;

            //_factory = factory;
        }

        // public async Task HandleUpdateAsync(ITgButtonCallback update)\\ интерфейс ITgButtonCallback был  как пример пример для мока
        // mock<ITgButtonCallback>.Set(_=> _.IsStart()).Returns(true);
        public async Task HandleUpdateAsync(Update update)

        // BeutiyPussyCallbeack(Update updte)
        // bool IsBotStart()
        // long GetUserId()
        // bool IsValid() // проверка что не содержит цифр и тд
        // bool IsMenuButton() // categoryMenu, brandMenu -  проверка, что выбрал именно меню
        // bool IsValueButton() // sephora, dildo, elf  -куда мы идем, чтобы проверить валидность кнопки?проверка на знаки и буквы?
        // string GetName() // метод отвчает за получение название кнопки?  эти методы объединить в один интерфейс ? не могу понять что от чего будет зависеть((
        //
        { 
            var userId =  _tgButtonCallback.GetUserId(update);// пока пришла к такому решению, все таки протестировать, чтобы посмотеть как ведут себя переменные
            var chatId = _tgButtonCallback.GetChatId(update);

            var userState = new UserSearchState(userId);
            await _userStateRepository.CreateAsync(userState);

            var currentState = await _userStateRepository.GetForUser(userId); // !!использование нового метода для установки актуального статуса
                                                                 //currentState.SetBrand(brand);               
            await _userStateRepository.UpdateAsync(currentState);

            if (_tgButtonCallback.IsStart) // именно кнопка старт
            {
                await _telegramSender.SendMainMenu(chatId);
                return;
                
            }
            if (_tgButtonCallback.IsValueMenuMessage) // какой то вариант из меню
            {
                var nameOfButton = _tgButtonCallback.GetNameOfButton(update);
                if (nameOfButton == "BrandMenu")
                {
                    await _telegramSender.SendBrandMenu(chatId);
                    return;
                }
                if( nameOfButton == "BrandMenu")
                {
                    await _telegramSender.SendCategoryMenu(chatId);
                    return;
                }

               

            }

             //if (messageText != null && messageText.Contains("/start")  || buttonName == "StartNewSearch")
            //{

            //    var userState = new UserSearchState(userId);
            //    await _userStateRepository.CreateAsync(userState);
            //    var currentState = await _userStateRepository.GetForUser(userId); // !!использование нового метода для установки актуального статуса
            //    //currentState.SetBrand(brand);               
            //    await _userStateRepository.UpdateAsync(currentState);

               



            //    //var startedForUserId = update.Message.From.Id;
            //    //var mainMenuHandler = _buttonHandlers["MainMenu"];
            //    //await mainMenuHandler.SendMenuToTelegramHandle(startedForUserId);
            //    //_logger.LogInformation(messageText);
            //    //return;
            //}
            if (update.Type != UpdateType.CallbackQuery)
            {
                throw new Exception("The user did not send a message");
            }

            //var callbackId = update.CallbackQuery.Id;
            //var userResponse = update.CallbackQuery?.Data;
            //;
            //var chatId = GetChatIdOrDefalt(userId);

            //if (buttonName == "Back")
            //{
            //    // await um.GetSearchStateForUser(userId);
            //    // await um.StepBack();
                                        
            //    // var nextMenu = userStateManager.GetNextMenu();
            //    // await tgSender.SendMenu(nextMenu); \\ аналог для выбора меню 
            //    _userStateManager.MethodBackOnOneStep(status);
            //    var lastMenu = _userStateManager.GetPreviousStep().ToString();
            //    var lastMenuHandler = _buttonHandlers[lastMenu];
            //    await lastMenuHandler.SendMenuToTelegramHandle(chatId);
            //    return;

            //}

            //if (buttonName == "BrandMenu")
            //{

            //    _currentMenuStatus = MenuStatus.BrandMenu;
            //    var handler = _buttonHandlers[buttonName];
            //    await handler.SendMenuToTelegramHandle(chatId);
            //    return;
            //}

            //if (buttonName == "CategoryMenu")
            //{
            //    _currentMenuStatus = MenuStatus.CategoryMenu;
            //    var handler = _buttonHandlers[buttonName];
            //    await handler.SendMenuToTelegramHandle(chatId);
            //    return;
            //}

            //var botState = _userResponsesToChat[userId];

            //if (buttonName.StartsWith("Brand_")) //  var botButton = new BotButton(update); // как мы понимаем какй пришел ответ, если пришла строка 
            //                                     // а не enum?
            //                                     // It's a new wrapper around update.CallbackQuery
            //                                     // check the button type, same as buttonName.StartsWith("Brand_") but hidden
            //                                     //if (botButton.IsBrand) / как мы понимаем какй пришел ответ, если пришла строка 
            //                                     // а не enum?Сделать проверку, если ответ не содержит "Back", то записываем ответ в базу и выдаем следующее меню?
            //                                     // класс BotButton  это аналог класса class tgSender

            //{

            //    botState.ChatId = update.CallbackQuery!.From.Id;
            //    var brand = buttonName.Replace("Brand_", "");


            //    var currentState = await _userStateRepository.GetForUser(userId); // !!использование нового метода для установки актуального статуса
            //    currentState.SetBrand(brand);
            //    await _userStateRepository.UpdateAsync(currentState);


            //    // await _userStateRepository.SetBrandForUser(userId); вместо методов, котоорые представлены ниже

            //    _currentMenuStatus = MenuStatus.BrandChosen;
            //    _userStateManager.SetBrand(MenuStatus.BrandChosen);
            //    string resultOfMenu = _userStateManager.GetNextStep("BrandChosen");

            //    if (resultOfMenu != "ReadyToResult")
            //    {
            //        await _buttonHandlers[resultOfMenu].SendMenuToTelegramHandle(chatId);
            //        return;
            //    }

            //    botState.PropertiesAreFilled();
            //    var result = SendAllResult(chatId, botState);
            //    _logger.LogInformation($"2. Received a  button'{buttonName}' userID {userId} and user First Name {firstName} and chatID {chatId}   и callbackId {callbackId}, userResponse {userResponse} ");

            //    await Task.Delay(3000);

            //    var handler = _buttonHandlers["ReturnToMainMenu"];
            //    await handler.SendMenuToTelegramHandle(chatId);
            //    return;
            //}

            //if (buttonName.StartsWith("Category_")) // для брендов из БД как мы можем понять что это именно название какого-то бренда, а не кнопка Back&&
            //{
            //    botState.ChatId = update.CallbackQuery!.From.Id;
            //    botState.Category = buttonName.Replace("Category_", "");
            //    _currentMenuStatus = MenuStatus.CategoryChosen;
            //    _userStateManager.SetCategory(MenuStatus.CategoryChosen);
            //    string resultOfMenu = _userStateManager.GetNextStep("CategoryChosen");

            //    if (resultOfMenu != "ReadyToResult")
            //    {

            //        await _buttonHandlers[resultOfMenu].SendMenuToTelegramHandle(chatId);
            //        return;
            //    }
            //    botState.PropertiesAreFilled();
            //    var result = SendAllResult(chatId, botState);
            //    _logger.LogInformation($"3. Received a  button'{buttonName}' userID {userId} and user First Name {firstName} and chatID {chatId}   и callbackId {callbackId}, userResponse {userResponse} ");
            //    await Task.Delay(3000);
            //    var handler = _buttonHandlers["ReturnToMainMenu"];
            //    await handler.SendMenuToTelegramHandle(chatId);
            //    return;
            //}
        }
        //private async Task<IEnumerable<string>> SendAllResult(long chatId, BotState botState)
        //{
        //    string brand = botState.Brand;
        //    string category = botState.Category;
        //    var products = await _actionsHandler.GetFilteredProductsMessage(brand, category);
        //    foreach (var product in products)
        //    {
        //        await _botClient.SendTextMessageAsync(chatId, product, parseMode: ParseMode.Markdown);
        //    }
        //    return products;
        //}
        //private long GetChatIdOrDefalt(long userId)
        //{
        //    if (!_userResponsesToChat.ContainsKey(userId))
        //    {
        //        return userId;
        //    }
        //    var botState = _userResponsesToChat[userId];
        //    return botState.ChatId;
        //}
    }
}
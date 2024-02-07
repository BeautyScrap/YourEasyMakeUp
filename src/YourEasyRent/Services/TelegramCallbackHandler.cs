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

namespace YourEasyRent.Services
{
    public class TelegramCallbackHandler : ITelegramCallbackHandler
    {
        private readonly ITelegramBotClient _botClient;
        private ITelegramActionsHandler _actionsHandler;
        private Dictionary<string, IButtonHandler> _buttonHandlers;
        private Dictionary<long, long> _usersToChats = new();
        //private BotState _currentBotState = BotState.Initial; 
        //  создать еще один дикшенари для сохранения сессии и ответов пользователей для передачи данных в базу данных
        //private BotState _currentBotState = BotState.Initial; //  инициализация  переменной _currentState для отслеживания текущего состояния  бота.

        private string _currentBrand = "";
        private string _currentCategory = "";


        public TelegramCallbackHandler(ITelegramBotClient botClient, ITelegramActionsHandler actionsHandler)
        {
            _botClient = botClient;
            _actionsHandler = actionsHandler;
            _buttonHandlers = new Dictionary<string, IButtonHandler>()
            {
                { "MainMenu", new MainMenuButtonHandler(_botClient) },
                { "BrandMenu", new BrandButtonHandler(_botClient) }
            };
            //{"CategoryMenu",  new CategoryButtonHandler() },
            //{"ReturnToMenu", new ReturnButtonHandler() },
            //{"Back", new BackReturnHandler() }

            // дописать еще CategoryMenu  и общие кнопки типо back and return
        }

        public async Task HandleUpdateAsync(Update update)
        {
            var messageText = update.Message?.Text;
            var firstName = update.Message.From.FirstName;            
            //var userID = update.Message.From.Id;
            var chatId = update.Message.Chat.Id;
            //var callbackId = update.CallbackQuery.Id;
            //var userIdcb = update.CallbackQuery!.From.Id;
           // var userchatId = _usersToChats[userIdcb];
           // var buttonName = update.CallbackQuery.Data;
            Console.WriteLine($"1. Received a '{messageText}' message userID  and user First Name {firstName} and chatID {chatId} .");

            //Task<Message> action;
            if (messageText != null && messageText.Contains("/start"))
            {
                var startedForUserId = update.Message.From.Id;
                _usersToChats[startedForUserId] = update.Message.Chat.Id;//  почему то в chatID  подставляет значение из UserID, возможно поэтому и не может отправить меню в нужный чат

                var mainMenuHandler = _buttonHandlers["MainMenu"];
                await mainMenuHandler.SendMenuToTelegramHandle(startedForUserId);
                Console.WriteLine($"3. Received a '{messageText}' message userID  and user First Name {firstName} and chatID {chatId} .");
                return;
                
                //return; //
                //await _botClient.SendTextMessageAsync(chatId, "Hello! Let me find some cosmetics for you!", cancellationToken: cancellationToken);
                //await _botClient.SendTextMessageAsync(chatId, "Mein menu", replyMarkup: _telegramMenu.CallMeinMenu());          
            }
            if (update.Type != UpdateType.CallbackQuery)
            {
                throw new Exception("The user did not send a message");
            }

            var userIdcb = update.CallbackQuery!.From.Id;
            var userchatId = _usersToChats[userIdcb];
            var buttonName = update.CallbackQuery.Data;
            //var userId = update.CallbackQuery!.From.Id;
            //var chatId = _usersToChats[userId];
            Console.WriteLine($"4. Received a  button'{buttonName}' userID {userIdcb} and user First Name {firstName} and chatID {userchatId} .");
            //var buttonName = update.CallbackQuery.Data;
            var handler = _buttonHandlers[buttonName];
            await handler.SendMenuToTelegramHandle(chatId);

        }





        //public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
        //{
        //    if (update.Type == UpdateType.Message && update.Message!.Type == MessageType.Text)
        //    {
        //        //var chatId = update.Message.Chat.Id;
        //        //var messageText = update.Message.Text;
        //        //var firstName = update.Message.From.FirstName;

        //        //Console.WriteLine($"Received a '{messageText}' message in chat {chatId} and user name {firstName}.");

        //        //if (messageText.Contains("/start"))
        //        //{
        //        //    _currentBotState = BotState.Initial;
        //        //    Console.WriteLine($"Received a '{messageText}' message in chat {chatId} and user name {firstName}.");
        //        //    await _botClient.SendTextMessageAsync(chatId, "Hello! Let me find some cosmetics for you!", cancellationToken: cancellationToken);
        //        //    await _botClient.SendTextMessageAsync(chatId, "Mein menu", replyMarkup: _telegramMenu.Mein);
        //        //    return;
        //        //}
        //    }

        //    else if (update.Type == UpdateType.CallbackQuery)
        //    {

        //        var callbackQuery = update.CallbackQuery;

        //        var callbackQueryChatId = callbackQuery.Message.Chat.Id;
        //        var callbackQueryNameOfButton = callbackQuery.Data;
        //        var firstName = callbackQuery.Message.Chat.Username;

        //        //if(callbackQuery == null)
        //        // ???? maybe throw
        //        switch (_currentBotState)
        //        {
        //            case BotState.Initial:
        //                // HandleInit(...);
        //                if (callbackQuery != null)
        //                {
        //                    if (callbackQuery?.Data == "Brand")
        //                    {
        //                        _currentBotState = BotState.BrandSelected;
        //                        Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
        //                        await _botClient.SendTextMessageAsync(callbackQueryChatId, "Сhoose the brand:", replyMarkup: _telegramMenu.Brand);
        //                    }
        //                    else if (callbackQuery!.Data == "Category")
        //                    {
        //                        _currentBotState = BotState.CategorySelected;
        //                        Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
        //                        await _botClient.SendTextMessageAsync(callbackQueryChatId, "Сhoose the category:", replyMarkup: _telegramMenu.Category);
        //                    }

        //                }
        //                break;

        //            case BotState.BrandSelected:
        //                // HandleBrandSelectButton(...)
        //                if (callbackQuery != null)
        //                {
        //                    var callbackQueryTime = callbackQuery.Message.Date; // Это время создания сообщения, на основе которого будет рассчитываться возраст запроса.
        //                    DateTime currentTime = DateTime.UtcNow;  // Это текущее время, которое будет использоваться для сравнения с временем создания запроса.
        //                    TimeSpan queryTime = currentTime - callbackQueryTime; // Этот интервал позволяет нам определить, сколько времени прошло с момента создания запроса.

        //                    if (queryTime.TotalSeconds > 70)
        //                    {
        //                        await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "This action is no longer valid.");
        //                    }
        //                    else
        //                    {
        //                        // MongoDB/UserChats -
        //                        // /start CreateNewChat(chatid)
        //                        // 123123123 : {}
        //                        // UpdateCategory(chatid)
        //                        // 123123123 : {"Category":"Maybelline"}
        //                        // google callbackquey example
        //                        // 

        //                        if (callbackQuery!.Data == "Maybelline")
        //                        {
        //                            _currentBrand = "Maybelline";
        //                            _currentBotState = BotState.CategorySelected;
        //                            Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
        //                            await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
        //                            await _botClient.SendTextMessageAsync(callbackQueryChatId, "Now choose the category of the product", replyMarkup: _telegramMenu.Category);
        //                        }

        //                        else if (callbackQuery!.Data == "TARTE")
        //                        {
        //                            _currentBrand = "TARTE";
        //                            _currentBotState = BotState.CategorySelected;
        //                            Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
        //                            await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
        //                            await _botClient.SendTextMessageAsync(callbackQueryChatId, "Now choose the category of the product", replyMarkup: _telegramMenu.Category);

        //                        }

        //                        else if (callbackQuery!.Data == "MAC")
        //                        {
        //                            _currentBrand = "MAC";
        //                            _currentBotState = BotState.CategorySelected;
        //                            Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
        //                            await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
        //                            await _botClient.SendTextMessageAsync(callbackQueryChatId, "Now choose the category of the product", replyMarkup: _telegramMenu.Category);

        //                        }

        //                        else if (callbackQuery!.Data == "FENTY_BEAUTY")
        //                        {
        //                            _currentBrand = "FENTY BEAUTY";
        //                            _currentBotState = BotState.CategorySelected;
        //                            Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
        //                            await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);

        //                            await _botClient.SendTextMessageAsync(callbackQueryChatId, "Now choose the category of the product", replyMarkup: _telegramMenu.Category);
        //                        }

        //                        else if (callbackQuery!.Data == "Back")
        //                        {
        //                            _currentBotState = BotState.Initial;
        //                            Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
        //                            await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
        //                            await _botClient.SendTextMessageAsync(callbackQueryChatId, "Main Menu", replyMarkup: _telegramMenu.Mein);
        //                        }
        //                    }
        //                }
        //                break;

        //            case BotState.CategorySelected:

        //                if (callbackQuery != null)
        //                {
        //                    DateTime callbackQueryTime = callbackQuery.Message.Date;
        //                    DateTime currentTime = DateTime.UtcNow;
        //                    TimeSpan queryTime = currentTime - callbackQueryTime;

        //                    if (queryTime.TotalSeconds > 70)
        //                    {
        //                        await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "This action is no longer valid.");
        //                    }
        //                    else
        //                    {

        //                        if (callbackQuery!.Data == "Mascara")
        //                        {
        //                            _currentCategory = "Mascara";
        //                            await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
        //                            var productMessages = await _actionsHandler.GetFilteredProductsMessage(_currentBrand, _currentCategory);
        //                            foreach (var productMessage in productMessages)
        //                            {
        //                                await _botClient.SendTextMessageAsync(callbackQueryChatId, productMessage, parseMode: ParseMode.Markdown);
        //                            }

        //                            await _botClient.SendTextMessageAsync(callbackQueryChatId, "Find new product", replyMarkup: _telegramMenu.ReturnToMeinMenu);
        //                            _currentBotState = BotState.ReturnToMeinMenu;
        //                            Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");

        //                        }

        //                        else if (callbackQuery!.Data == "Foundation")
        //                        {
        //                            _currentCategory = "Foundation";
        //                            await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
        //                            var productMessages = await _actionsHandler.GetFilteredProductsMessage(_currentBrand, _currentCategory);
        //                            foreach (var productMessage in productMessages)
        //                            {
        //                                await _botClient.SendTextMessageAsync(callbackQueryChatId, productMessage, parseMode: ParseMode.Markdown);
        //                            }
        //                            await _botClient.SendTextMessageAsync(callbackQueryChatId, "Find new product", replyMarkup: _telegramMenu.ReturnToMeinMenu);
        //                            _currentBotState = BotState.ReturnToMeinMenu;
        //                            Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
        //                        }

        //                        else if (callbackQuery!.Data == "Concealer")
        //                        {
        //                            _currentCategory = "Concealer";
        //                            await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
        //                            var productMessages = await _actionsHandler.GetFilteredProductsMessage(_currentBrand, _currentCategory);
        //                            foreach (var productMessage in productMessages)
        //                            {
        //                                await _botClient.SendTextMessageAsync(callbackQueryChatId, productMessage, parseMode: ParseMode.Markdown);
        //                            }
        //                            await _botClient.SendTextMessageAsync(callbackQueryChatId, "Find new product", replyMarkup: _telegramMenu.ReturnToMeinMenu);
        //                            _currentBotState = BotState.ReturnToMeinMenu;
        //                            Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
        //                        }

        //                        else if (callbackQuery!.Data == "Blush")
        //                        {
        //                            _currentCategory = "Blush";
        //                            await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
        //                            var productMessages = await _actionsHandler.GetFilteredProductsMessage(_currentBrand, _currentCategory);
        //                            foreach (var productMessage in productMessages)
        //                            {
        //                                await _botClient.SendTextMessageAsync(callbackQueryChatId, productMessage, parseMode: ParseMode.Markdown);
        //                            }
        //                            await _botClient.SendTextMessageAsync(callbackQueryChatId, "Find new product", replyMarkup: _telegramMenu.ReturnToMeinMenu);
        //                            _currentBotState = BotState.ReturnToMeinMenu;
        //                            Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
        //                        }

        //                        else if (callbackQuery!.Data == "Highlighter")
        //                        {
        //                            _currentCategory = "Consealer";
        //                            await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
        //                            var productMessages = await _actionsHandler.GetFilteredProductsMessage(_currentBrand, _currentCategory);
        //                            foreach (var productMessage in productMessages)
        //                            {
        //                                await _botClient.SendTextMessageAsync(callbackQueryChatId, productMessage, parseMode: ParseMode.Markdown);
        //                            }
        //                            await _botClient.SendTextMessageAsync(callbackQueryChatId, "Find new product", replyMarkup: _telegramMenu.ReturnToMeinMenu);
        //                            _currentBotState = BotState.ReturnToMeinMenu;
        //                            Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
        //                        }

        //                        else if (callbackQuery!.Data == "Eyeshadow")
        //                        {
        //                            _currentCategory = "Eyeshadow";
        //                            await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
        //                            var productMessages = await _actionsHandler.GetFilteredProductsMessage(_currentBrand, _currentCategory);
        //                            foreach (var productMessage in productMessages)
        //                            {
        //                                await _botClient.SendTextMessageAsync(callbackQueryChatId, productMessage, parseMode: ParseMode.Markdown);
        //                            }
        //                            await _botClient.SendTextMessageAsync(callbackQueryChatId, "Find new product", replyMarkup: _telegramMenu.ReturnToMeinMenu);
        //                            _currentBotState = BotState.ReturnToMeinMenu;
        //                            Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
        //                        }

        //                        else if (callbackQuery!.Data == "Brow pencils")
        //                        {
        //                            _currentCategory = "Brow pencils";
        //                            await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
        //                            var productMessages = await _actionsHandler.GetFilteredProductsMessage(_currentBrand, _currentCategory);
        //                            foreach (var productMessage in productMessages)
        //                            {
        //                                await _botClient.SendTextMessageAsync(callbackQueryChatId, productMessage, parseMode: ParseMode.Markdown);
        //                            }
        //                            await _botClient.SendTextMessageAsync(callbackQueryChatId, "Find new product", replyMarkup: _telegramMenu.ReturnToMeinMenu);
        //                            _currentBotState = BotState.ReturnToMeinMenu;
        //                            Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
        //                        }

        //                        else if (callbackQuery!.Data == "Lipstick")
        //                        {
        //                            _currentCategory = "Lipstick";
        //                            await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
        //                            var productMessages = await _actionsHandler.GetFilteredProductsMessage(_currentBrand, _currentCategory);
        //                            foreach (var productMessage in productMessages)
        //                            {
        //                                await _botClient.SendTextMessageAsync(callbackQueryChatId, productMessage, parseMode: ParseMode.Markdown);
        //                            }
        //                            await _botClient.SendTextMessageAsync(callbackQueryChatId, "Find new product", replyMarkup: _telegramMenu.ReturnToMeinMenu);
        //                            _currentBotState = BotState.ReturnToMeinMenu;
        //                            Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
        //                        }

        //                        else if (callbackQuery!.Data == "Back")
        //                        {
        //                            _currentBotState = BotState.Initial;
        //                            Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
        //                            await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
        //                            await _botClient.SendTextMessageAsync(callbackQueryChatId, "Main Menu", replyMarkup: _telegramMenu.Mein);
        //                        }
        //                    }
        //                }
        //                break;

        //            case BotState.ReturnToMeinMenu:
        //                if (callbackQuery!.Data == "Return_To_MeinMenu")
        //                {
        //                    _currentBotState = BotState.Initial;
        //                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
        //                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Welcome back to the main menu.", replyMarkup: _telegramMenu.Mein);
        //                    Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
        //                }
        //                break;
        //        }

        //    }

        }
    }



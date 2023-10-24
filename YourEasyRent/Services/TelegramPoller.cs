using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using YourEasyRent.Entities;
using System.Data;

namespace YourEasyRent.Services
{
    public class TelegramPoller
    {
        private readonly ITelegramBotClient _botClient;
        //private readonly CancellationTokenSource _cts = new();
        private ITelegramActionsHandler _actionsHandler;
        private ITelegramMenu _telegramMenu;
        private BotState _currentBotState = BotState.Initial; //  инициализация  переменной _currentState для отслеживания текущего состояния  бота.

        private string _currentBrand = "";
        private string _currentCategory = "";



        public TelegramPoller(ITelegramBotClient botClient, ITelegramActionsHandler actionsHandler, ITelegramMenu telegramMenu)
        {
            _botClient = botClient;
            _actionsHandler = actionsHandler;
            _telegramMenu = telegramMenu;
        }

        public void StartReceivingMessages()
        {
            var receiverOptions = new ReceiverOptions  //  создаем новый объект  ReceiverOptions для настройки параметров получения обновлений от Telegram API
            {
                AllowedUpdates = Array.Empty<UpdateType>() //  все допустимые обновления будут приниматься в массив UpdateType
            };

            _botClient.StartReceiving // вызываем метод StartReceiving, чтобы начать процесс получения обновлений от Telegram.  В методе StartReceiving определены обработчики обновлений (updateHandler и pollingErrorHandler), опции получения (receiverOptions) и токен отмены (cancellationToken),
                (
                updateHandler: async (_, update, cancellationToken) => { await HandleUpdateAsync(update, cancellationToken); },

                pollingErrorHandler: async (bot, exception, cancellationToken) => { await HandlePollingErrorAsync(bot, exception, cancellationToken); },

                receiverOptions: receiverOptions
                //cancellationToken: _cts.Token
                );

            var me = _botClient.GetMeAsync().Result;
            Console.WriteLine($"Start listening for @{me.Username}"); // получаем информация о боте с использованием _botClient.GetMeAsync() и выводится его имя пользователя в консоль для отображения информации о начале прослушивания обновлений.

        }


        public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message!.Type == MessageType.Text)
            {
                var chatId = update.Message.Chat.Id;
                var messageText = update.Message.Text;
                var firstName = update.Message.From.FirstName;

                Console.WriteLine($"Received a '{messageText}' message in chat {chatId} and user name {firstName}.");
                #region [First Message]



                if (messageText.Contains("/start"))
                {
                    await _botClient.SendTextMessageAsync(chatId, "Hello! Let me find some cosmetics for you!", cancellationToken: cancellationToken);
                    await _botClient.SendTextMessageAsync(chatId, "Mein menu", replyMarkup: _telegramMenu.meinMenu);
                    _currentBotState = BotState.Initial;
                    Console.WriteLine($"Received a '{messageText}' message in chat {chatId} and user name {firstName}.");
                    return;
                }
            }
            #endregion
            else if (update.Type == UpdateType.CallbackQuery)
            {

                var callbackQuery = update.CallbackQuery;

                var callbackQueryChatId = callbackQuery.Message.Chat.Id;
                var callbackQueryNameOfButton = callbackQuery.Data;
                var firstName = callbackQuery.Message.From.FirstName;

                switch (_currentBotState)
                {
                    case BotState.Initial:
                        if (callbackQuery != null)
                        {
                            DateTime callbackQueryTime = callbackQuery.Message.Date; // Это время создания сообщения, на основе которого будет рассчитываться возраст запроса.
                            DateTime currentTime = DateTime.UtcNow;  // Это текущее время, которое будет использоваться для сравнения с временем создания запроса.
                            TimeSpan queryTime = currentTime - callbackQueryTime; // Этот интервал позволяет нам определить, сколько времени прошло с момента создания запроса.

                            if (queryTime.TotalSeconds > 70)
                            {
                                await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "This action is no longer valid.");
                            }
                            else
                            {
                                if (callbackQuery?.Data == "Brand")
                                {
                                    Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
                                    _currentBotState = BotState.BrandSelected;
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Сhoose the brand:", replyMarkup: _telegramMenu.brandMenu);
                                }
                                else if (callbackQuery!.Data == "Category")
                                {
                                    Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
                                    _currentBotState = BotState.CategorySelected;
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Сhoose the category:", replyMarkup: _telegramMenu.categoryMenu);
                                }
                                //else if (callbackQuery!.Data == "Back")
                                //{
                                //    _currentBotState = BotState.Initial;
                                //    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Main Menu", replyMarkup: _telegramMenu.meinMenu);
                                //}
                            }
                        }
                        break;

                    case BotState.BrandSelected:
                        if (callbackQuery != null)
                        {
                            DateTime callbackQueryTime = callbackQuery.Message.Date; // Это время создания сообщения, на основе которого будет рассчитываться возраст запроса.
                            DateTime currentTime = DateTime.UtcNow;  // Это текущее время, которое будет использоваться для сравнения с временем создания запроса.
                            TimeSpan queryTime = currentTime - callbackQueryTime; // Этот интервал позволяет нам определить, сколько времени прошло с момента создания запроса.

                            if (queryTime.TotalSeconds > 70)
                            {
                                await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "This action is no longer valid.");
                            }
                            else
                            {

                                if (callbackQuery!.Data == "Maybelline")
                                {
                                    Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
                                    _currentBrand = "Maybelline";
                                    _currentBotState = BotState.CategorySelected;
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Now choose the category of the product", replyMarkup: _telegramMenu.categoryMenu);
                                }

                                else if (callbackQuery!.Data == "Loreal")
                                {
                                    _currentBrand = "Loreal";
                                    _currentBotState = BotState.CategorySelected;
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Now choose the category of the product", replyMarkup: _telegramMenu.categoryMenu);

                                }

                                else if (callbackQuery!.Data == "MAC")
                                {
                                    _currentBrand = "MAC";
                                    _currentBotState = BotState.CategorySelected;
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Now choose the category of the product", replyMarkup: _telegramMenu.categoryMenu);

                                }

                                else if (callbackQuery!.Data == "FENTY_BEAUTY")
                                {
                                    _currentBrand = "FENTY BEAUTY";
                                    _currentBotState = BotState.CategorySelected;
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Now choose the category of the product", replyMarkup: _telegramMenu.categoryMenu);
                                }

                                else if (callbackQuery!.Data == "Back")
                                {
                                    _currentBotState = BotState.Initial;
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Main Menu", replyMarkup: _telegramMenu.meinMenu);
                                }
                            }
                        }
                        break;

                    case BotState.CategorySelected:

                        if (callbackQuery != null)
                        {
                            DateTime callbackQueryTime = callbackQuery.Message.Date; // Это время создания сообщения, на основе которого будет рассчитываться возраст запроса.
                            DateTime currentTime = DateTime.UtcNow;  // Это текущее время, которое будет использоваться для сравнения с временем создания запроса.
                            TimeSpan queryTime = currentTime - callbackQueryTime; // Этот интервал позволяет нам определить, сколько времени прошло с момента создания запроса.

                            if (queryTime.TotalSeconds > 70)
                            {
                                await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "This action is no longer valid.");
                            }
                            else
                            {

                                if (callbackQuery!.Data == "Mascara")
                                {
                                    
                                    _currentCategory = "Mascara";
                                    //await _botClient.SendTextMessageAsync(callbackQueryChatId, "Check your request: \n");
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    var productMessages = await _actionsHandler.GetFilteredProductsMessage(_currentBrand, _currentCategory);
                                    foreach (var productMessage in productMessages)
                                    {
                                        await _botClient.SendTextMessageAsync(callbackQueryChatId, productMessage, parseMode: ParseMode.Markdown);
                                    }

                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Find new product", replyMarkup: _telegramMenu.ReturnToMeinMenu);
                                    _currentBotState = BotState.ReturnToMeinMenu;
                                    Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");

                                    //await _botClient.SendTextMessageAsync(callbackQueryChatId, "Find new product", replyMarkup: _telegramMenu.ReturnToMeinMenu);

                                }

                                else if (callbackQuery!.Data == "Foundation")
                                {
                                    _currentCategory = "Foundation";
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    // await _botClient.SendTextMessageAsync(callbackQueryChatId, "Check your request: \n");
                                    var productMessages = await _actionsHandler.GetFilteredProductsMessage(_currentBrand, _currentCategory);
                                    foreach (var productMessage in productMessages)
                                    {
                                        await _botClient.SendTextMessageAsync(callbackQueryChatId, productMessage, parseMode: ParseMode.Markdown);
                                    }
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Find new product", replyMarkup: _telegramMenu.ReturnToMeinMenu);
                                    _currentBotState = BotState.ReturnToMeinMenu;
                                }
                                else if (callbackQuery!.Data == "Back")
                                {
                                    _currentBotState = BotState.Initial;
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Main Menu", replyMarkup: _telegramMenu.meinMenu);
                                }
                            }
                        }
                        break;

                    case BotState.ReturnToMeinMenu:
                        if (callbackQuery!.Data == "Return_To_MeinMenu")
                        {
                            _currentBotState = BotState.Initial;
                            await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                            await _botClient.SendTextMessageAsync(callbackQueryChatId, "Welcome back to the main menu.", replyMarkup: _telegramMenu.meinMenu);
                        }

                        break;


                }

            }

        }


        private Task HandlePollingErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}

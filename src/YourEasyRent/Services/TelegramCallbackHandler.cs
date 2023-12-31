﻿using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using YourEasyRent.Entities;
using System.Data;

namespace YourEasyRent.Services
{
    public class TelegramCallbackHandler : ITelegramCallbackHandler
    {
        private readonly ITelegramBotClient _botClient;
        private ITelegramActionsHandler _actionsHandler;
        private ITelegramMenu _telegramMenu;
        private BotState _currentBotState = BotState.Initial; //  инициализация  переменной _currentState для отслеживания текущего состояния  бота.

        private string _currentBrand = "";
        private string _currentCategory = "";


        public TelegramCallbackHandler(ITelegramBotClient botClient, ITelegramActionsHandler actionsHandler, ITelegramMenu telegramMenu)
        {
            _botClient = botClient;
            _actionsHandler = actionsHandler;
            _telegramMenu = telegramMenu;
        }

        public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message!.Type == MessageType.Text)
            {
                var chatId = update.Message.Chat.Id;
                var messageText = update.Message.Text;
                var firstName = update.Message.From.FirstName;

                Console.WriteLine($"Received a '{messageText}' message in chat {chatId} and user name {firstName}.");

                if (messageText.Contains("/start"))
                {
                    _currentBotState = BotState.Initial;
                    Console.WriteLine($"Received a '{messageText}' message in chat {chatId} and user name {firstName}.");
                    await _botClient.SendTextMessageAsync(chatId, "Hello! Let me find some cosmetics for you!", cancellationToken: cancellationToken);
                    await _botClient.SendTextMessageAsync(chatId, "Mein menu", replyMarkup: _telegramMenu.Mein);
                    return;
                }
            }

            else if (update.Type == UpdateType.CallbackQuery)
            {

                var callbackQuery = update.CallbackQuery;

                var callbackQueryChatId = callbackQuery.Message.Chat.Id;
                var callbackQueryNameOfButton = callbackQuery.Data;
                var firstName = callbackQuery.Message.Chat.Username;

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
                                    _currentBotState = BotState.BrandSelected;
                                    Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Сhoose the brand:", replyMarkup: _telegramMenu.Brand);
                                }
                                else if (callbackQuery!.Data == "Category")
                                {
                                    _currentBotState = BotState.CategorySelected;
                                    Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Сhoose the category:", replyMarkup: _telegramMenu.Category);
                                }
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
                                    _currentBrand = "Maybelline";
                                    _currentBotState = BotState.CategorySelected;
                                    Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Now choose the category of the product", replyMarkup: _telegramMenu.Category);
                                }

                                else if (callbackQuery!.Data == "TARTE")
                                {
                                    _currentBrand = "TARTE";
                                    _currentBotState = BotState.CategorySelected;
                                    Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Now choose the category of the product", replyMarkup: _telegramMenu.Category);

                                }

                                else if (callbackQuery!.Data == "MAC")
                                {
                                    _currentBrand = "MAC";
                                    _currentBotState = BotState.CategorySelected;
                                    Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Now choose the category of the product", replyMarkup: _telegramMenu.Category);

                                }

                                else if (callbackQuery!.Data == "FENTY_BEAUTY")
                                {
                                    _currentBrand = "FENTY BEAUTY";
                                    _currentBotState = BotState.CategorySelected;
                                    Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Now choose the category of the product", replyMarkup: _telegramMenu.Category);
                                }

                                else if (callbackQuery!.Data == "Back")
                                {
                                    _currentBotState = BotState.Initial;
                                    Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Main Menu", replyMarkup: _telegramMenu.Mein);
                                }
                            }
                        }
                        break;

                    case BotState.CategorySelected:

                        if (callbackQuery != null)
                        {
                            DateTime callbackQueryTime = callbackQuery.Message.Date;
                            DateTime currentTime = DateTime.UtcNow;
                            TimeSpan queryTime = currentTime - callbackQueryTime;

                            if (queryTime.TotalSeconds > 70)
                            {
                                await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "This action is no longer valid.");
                            }
                            else
                            {

                                if (callbackQuery!.Data == "Mascara")
                                {
                                    _currentCategory = "Mascara";
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    var productMessages = await _actionsHandler.GetFilteredProductsMessage(_currentBrand, _currentCategory);
                                    foreach (var productMessage in productMessages)
                                    {
                                        await _botClient.SendTextMessageAsync(callbackQueryChatId, productMessage, parseMode: ParseMode.Markdown);
                                    }

                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Find new product", replyMarkup: _telegramMenu.ReturnToMeinMenu);
                                    _currentBotState = BotState.ReturnToMeinMenu;
                                    Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");

                                }

                                else if (callbackQuery!.Data == "Foundation")
                                {
                                    _currentCategory = "Foundation";
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    var productMessages = await _actionsHandler.GetFilteredProductsMessage(_currentBrand, _currentCategory);
                                    foreach (var productMessage in productMessages)
                                    {
                                        await _botClient.SendTextMessageAsync(callbackQueryChatId, productMessage, parseMode: ParseMode.Markdown);
                                    }
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Find new product", replyMarkup: _telegramMenu.ReturnToMeinMenu);
                                    _currentBotState = BotState.ReturnToMeinMenu;
                                    Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
                                }

                                else if (callbackQuery!.Data == "Concealer")
                                {
                                    _currentCategory = "Concealer";
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    var productMessages = await _actionsHandler.GetFilteredProductsMessage(_currentBrand, _currentCategory);
                                    foreach (var productMessage in productMessages)
                                    {
                                        await _botClient.SendTextMessageAsync(callbackQueryChatId, productMessage, parseMode: ParseMode.Markdown);
                                    }
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Find new product", replyMarkup: _telegramMenu.ReturnToMeinMenu);
                                    _currentBotState = BotState.ReturnToMeinMenu;
                                    Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
                                }

                                else if (callbackQuery!.Data == "Blush")
                                {
                                    _currentCategory = "Blush";
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    var productMessages = await _actionsHandler.GetFilteredProductsMessage(_currentBrand, _currentCategory);
                                    foreach (var productMessage in productMessages)
                                    {
                                        await _botClient.SendTextMessageAsync(callbackQueryChatId, productMessage, parseMode: ParseMode.Markdown);
                                    }
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Find new product", replyMarkup: _telegramMenu.ReturnToMeinMenu);
                                    _currentBotState = BotState.ReturnToMeinMenu;
                                    Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
                                }

                                else if (callbackQuery!.Data == "Highlighter")
                                {
                                    _currentCategory = "Consealer";
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    var productMessages = await _actionsHandler.GetFilteredProductsMessage(_currentBrand, _currentCategory);
                                    foreach (var productMessage in productMessages)
                                    {
                                        await _botClient.SendTextMessageAsync(callbackQueryChatId, productMessage, parseMode: ParseMode.Markdown);
                                    }
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Find new product", replyMarkup: _telegramMenu.ReturnToMeinMenu);
                                    _currentBotState = BotState.ReturnToMeinMenu;
                                    Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
                                }

                                else if (callbackQuery!.Data == "Eyeshadow")
                                {
                                    _currentCategory = "Eyeshadow";
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    var productMessages = await _actionsHandler.GetFilteredProductsMessage(_currentBrand, _currentCategory);
                                    foreach (var productMessage in productMessages)
                                    {
                                        await _botClient.SendTextMessageAsync(callbackQueryChatId, productMessage, parseMode: ParseMode.Markdown);
                                    }
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Find new product", replyMarkup: _telegramMenu.ReturnToMeinMenu);
                                    _currentBotState = BotState.ReturnToMeinMenu;
                                    Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
                                }

                                else if (callbackQuery!.Data == "Brow pencils")
                                {
                                    _currentCategory = "Brow pencils";
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    var productMessages = await _actionsHandler.GetFilteredProductsMessage(_currentBrand, _currentCategory);
                                    foreach (var productMessage in productMessages)
                                    {
                                        await _botClient.SendTextMessageAsync(callbackQueryChatId, productMessage, parseMode: ParseMode.Markdown);
                                    }
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Find new product", replyMarkup: _telegramMenu.ReturnToMeinMenu);
                                    _currentBotState = BotState.ReturnToMeinMenu;
                                    Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
                                }

                                else if (callbackQuery!.Data == "Lipstick")
                                {
                                    _currentCategory = "Lipstick";
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    var productMessages = await _actionsHandler.GetFilteredProductsMessage(_currentBrand, _currentCategory);
                                    foreach (var productMessage in productMessages)
                                    {
                                        await _botClient.SendTextMessageAsync(callbackQueryChatId, productMessage, parseMode: ParseMode.Markdown);
                                    }
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Find new product", replyMarkup: _telegramMenu.ReturnToMeinMenu);
                                    _currentBotState = BotState.ReturnToMeinMenu;
                                    Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
                                }

                                else if (callbackQuery!.Data == "Back")
                                {
                                    _currentBotState = BotState.Initial;
                                    Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                    await _botClient.SendTextMessageAsync(callbackQueryChatId, "Main Menu", replyMarkup: _telegramMenu.Mein);
                                }
                            }
                        }
                        break;

                    case BotState.ReturnToMeinMenu:
                        if (callbackQuery!.Data == "Return_To_MeinMenu")
                        {
                            _currentBotState = BotState.Initial;
                            await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                            await _botClient.SendTextMessageAsync(callbackQueryChatId, "Welcome back to the main menu.", replyMarkup: _telegramMenu.Mein);
                            Console.WriteLine($"Received a '{callbackQueryNameOfButton}' message in chat {callbackQueryChatId} and user name {firstName}.");
                        }
                        break;
                }

            }

        }
    }
}

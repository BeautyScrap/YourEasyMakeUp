using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


namespace YourEasyRent.Services
{
    public class TelegramPoller
    {
        private readonly TelegramBotClient _botClient;
        private readonly CancellationTokenSource _cts = new();

        public TelegramPoller(string token) // создаем конструктор для инициализации объектов класса TelegramBotSerices
        {
            _botClient = new TelegramBotClient(token);
        }

        public void StartReceivingMessages()
        {
            var receiverOptions =
                new ReceiverOptions //  создаем новый объект  ReceiverOptions для настройки параметров получения обновлений от Telegram API
                {
                    AllowedUpdates = Array.Empty<UpdateType>() //  все допустимые обновления будут приниматься в массив UpdateType
                };

            _botClient.StartReceiving // вызываем метод StartReceiving, чтобы начать процесс получения обновлений от Telegram.  В методе StartReceiving определены обработчики обновлений (updateHandler и pollingErrorHandler), опции получения (receiverOptions) и токен отмены (cancellationToken),
            (
                updateHandler: async (bot, update, cancellationToken) => { await HandleUpdateAsync(bot, update, cancellationToken); },
                pollingErrorHandler: async (bot, exception, cancellationToken) =>
                {
                    await HandlePollingErrorAsync(bot, exception, cancellationToken);
                },
                receiverOptions: receiverOptions,
                cancellationToken: _cts.Token
            );

            var me = _botClient.GetMeAsync().Result;
            Console.WriteLine(
                $"Start listening for @{me.Username}"); // получаем информация о боте с использованием _botClient.GetMeAsync() и выводится его имя пользователя в консоль для отображения информации о начале прослушивания обновлений.
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message!.Type == MessageType.Text)
            {
                var chatId = update.Message.Chat.Id;
                var messageText = update.Message.Text;
                var firstName = update.Message.From.FirstName;
                //var callbackQuery = update.CallbackQuery;
                Console.WriteLine($"Received a '{messageText}' message in chat {chatId} and user name {firstName}.");

                #region [First Message]

                if (messageText.Contains("/start"))
                {
                    await _botClient.SendTextMessageAsync(chatId, "Приветики! Давай я найду для тебя косметос!",
                        cancellationToken: cancellationToken);
                    await SendMainMenu(chatId);
                    Console.WriteLine($"Received a '{messageText}' message in chat {chatId} and user name {firstName}.");
                    return;
                }
            }

            #endregion

            if (update.Type == UpdateType.CallbackQuery)
            {
                var callbackQuery = update.CallbackQuery;
                var user = callbackQuery.From;
                var chatId = update.Message?.Chat.Id;

                if (callbackQuery.Data == "back")
                {
                    await SendMainMenu(callbackQuery.Message.Chat.Id);
                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);

                    return;
                }

                if (callbackQuery.Data == "Brand")
                {
                    await SendBrandMenu(callbackQuery.Message.Chat.Id);

                    return;
                }

                if (callbackQuery.Data == "Category")
                {
                    await SendCategoryMenu(callbackQuery.Message.Chat.Id);

                    return;
                }

                if (callbackQuery.Data == "Loreal")
                {
                    await SendCategoryMenu(callbackQuery.Message.Chat.Id);
                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                    await _botClient.SendTextMessageAsync(chatId, "Теперь выберите категорию продукта");
                    return;
                }
            }
        }

        async Task SendCategoryMenu(long chatId)
        {
            InlineKeyboardMarkup categoryMenu = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Foundation", callbackData: "Foundation"),
                    InlineKeyboardButton.WithCallbackData(text: "Consealer", callbackData: "Consealer"),
                },

                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Blush", callbackData: "Blush"),
                    InlineKeyboardButton.WithCallbackData(text: "Highlighter", callbackData: "Highlighter")
                },

                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Mascara", callbackData: "Mascara"),
                    InlineKeyboardButton.WithCallbackData(text: "Eyeshadow", callbackData: "Eyeshadow")
                },

                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Brow pencils", callbackData: "Brow pencils"),
                    InlineKeyboardButton.WithCallbackData(text: "Lipstick", callbackData: "Lipstick")
                },

                new[]
                    { InlineKeyboardButton.WithCallbackData(text: "back", callbackData: "back") }
            });


            await _botClient.SendTextMessageAsync(chatId, "Выбери категорию продукта", replyMarkup: categoryMenu);
        }

        async Task SendBrandMenu(long chatId)
        {
            InlineKeyboardMarkup brandMenu = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Loreal", callbackData: "Loreal"),
                    InlineKeyboardButton.WithCallbackData(text: "MAC", callbackData: "Mac")
                },

                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Maybelline", callbackData: "Maybelline"),
                    InlineKeyboardButton.WithCallbackData(text: "Fenty Beauty", callbackData: "Fenty_Beauty")
                },
                new[]
                    { InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "back") }
            });


            await _botClient.SendTextMessageAsync(chatId, "Выбери бренд", replyMarkup: brandMenu);
        }


        async Task SendMainMenu(long chatId)
        {
            InlineKeyboardMarkup mainMenu = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Бренд", callbackData: "Brand"),
                    InlineKeyboardButton.WithCallbackData(text: "Категория", callbackData: "Category")
                },

                new[]
                    { InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "back") }
            });

            await _botClient.SendTextMessageAsync(chatId, "Главное меню", replyMarkup: mainMenu);
        }


        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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
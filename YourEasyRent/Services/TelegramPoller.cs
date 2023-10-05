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
        private readonly ITelegramBotClient _botClient;
        private readonly CancellationTokenSource _cts = new();
        private ITelegramActionsHandler _botActionsHandler;
        private ITelegramMenu _telegramMenu;

        // this is a show case of idea
        private string _currentCategory = "";
        private string _currentBrand = "";

        public TelegramPoller(ITelegramBotClient telegramClient,
            ITelegramActionsHandler botActionsHandler,
            ITelegramMenu telegramMenu) // создаем конструктор для инициализации объектов класса TelegramBotSerices
        {
            _botClient = telegramClient;
            _botActionsHandler = botActionsHandler;
            _telegramMenu = telegramMenu;
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
                updateHandler: async (_, update, cancellationToken) => { await HandleUpdateAsync(update, cancellationToken); },
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

        // this is your equivalent of a controller action
        public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
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
                    await _botClient.SendTextMessageAsync(chatId, "Главное меню", replyMarkup: _telegramMenu.MainMenu);
                    Console.WriteLine($"Received a '{messageText}' message in chat {chatId} and user name {firstName}.");
                    return;
                }
            }

            #endregion

            if (update.Type == UpdateType.CallbackQuery)
            {
                var callbackQuery = update.CallbackQuery;
                var chatId = update.Message?.Chat.Id;
                // whats the difference by the way? I think they should be the same
                var callbackChatId = callbackQuery.Message.Chat.Id;

                switch (callbackQuery!.Data)
                {
                    case "back":
                        await _botClient.SendTextMessageAsync(callbackChatId, "Главное меню", replyMarkup: _telegramMenu.MainMenu);
                        await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);

                        return;
                    case "Brand":
                        await _botClient.SendTextMessageAsync(chatId, "Выбери бренд", replyMarkup: _telegramMenu.BrandMenu);

                        return;
                    case "Category":
                        await _botClient.SendTextMessageAsync(callbackChatId, "Выбери категорию продукта",
                            replyMarkup: _telegramMenu.CategoryMenu);
                        return;
                    case "Blush":
                        // I'm pretty sure there is a better way to handle buttons
                        _currentCategory = "Blush";
                        await _botClient.SendTextMessageAsync(callbackChatId, "Выбери категорию продукта",
                            replyMarkup: _telegramMenu.CategoryMenu);

                        // the idea is kind of - we choose a Brand, than a Category
                        var productsMessage = await _botActionsHandler.GetFilteredProductsMessage(_currentCategory, _currentBrand);
                        await _botClient.SendTextMessageAsync(callbackChatId, "Check this shit:\n" + productsMessage);
                        return;
                    case "Loreal":
                        // we store category in a class so this shit will work only for one person at a time
                        _currentBrand = "Loreal";

                        var menu = _telegramMenu.CategoryMenu;


                        await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                        await _botClient.SendTextMessageAsync(chatId, "Теперь выберите категорию продукта");
                        return;
                }
            }
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

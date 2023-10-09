using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;

namespace YourEasyRent.Services
{
    public class TelegramPoller
    {
        private readonly TelegramBotClient _botClient;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private ITelegramActionsHandler _actionsHandler;
        private ITelegramMenu _telegramMenu;


        private string _currentBrand = "";


        public TelegramPoller(TelegramBotClient botClient, CancellationTokenSource cts, ITelegramActionsHandler actionsHandler, ITelegramMenu telegramMenu)
        {
            _botClient = botClient;
            _cts = cts;
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
                updateHandler: async (bot, update, cancellationToken) => { await HandleUpdateAsync(bot, update, cancellationToken); },

                pollingErrorHandler: async (bot, exception, cancellationToken) => { await HandlePollingErrorAsync(bot, exception, cancellationToken); },

                receiverOptions: receiverOptions,
                cancellationToken: _cts.Token
                );

            var me = _botClient.GetMeAsync().Result;
            Console.WriteLine($"Start listening for @{me.Username}"); // получаем информация о боте с использованием _botClient.GetMeAsync() и выводится его имя пользователя в консоль для отображения информации о начале прослушивания обновлений.

        }


        
         public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
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
                    await _botClient.SendTextMessageAsync(chatId, "Приветики! Давай я найду для тебя косметос!", cancellationToken: cancellationToken);
                    await _botClient.SendTextMessageAsync(chatId, "Главное меню", replyMarkup: _telegramMenu.meinMenu);
                    Console.WriteLine($"Received a '{messageText}' message in chat {chatId} and user name {firstName}.");
                    return;
                }
            }
            #endregion
            if (update.Type == UpdateType.CallbackQuery)
            {
                
                var callbackQuery = update.CallbackQuery;              
                //var chatId = update.Message?.Chat.Id;
                var callbackQueryChatId = callbackQuery.Message.Chat.Id;

                switch (callbackQuery!.Data)
                {
                    case "Back":
                        await _botClient.SendTextMessageAsync(callbackQueryChatId, "Main Menu", replyMarkup: _telegramMenu.meinMenu);
                        await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                        return;

                    case "Brand":
                        await _botClient.SendTextMessageAsync(callbackQueryChatId, "Brand", replyMarkup: _telegramMenu.brandMenu);
                        return;

                    case "Category":
                        await _botClient.SendTextMessageAsync(callbackQueryChatId, "Category", replyMarkup: _telegramMenu.categoryMenu);
                        return;
                    case "Price":
                        await _botClient.SendTextMessageAsync(callbackQueryChatId, "Price", replyMarkup: _telegramMenu.priceMenu);
                        return;
                    case "Loreal":
                        _currentBrand = "Loreal";


                        return;






                }

                //if (callbackQuery.Data == "back")
                //{
                //    await SendMainMenu(callbackQuery.Message.Chat.Id);
                //    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);

                //    return;  
                //}

                //if (callbackQuery.Data == "Brand")
                //{
                //    await SendBrandMenu(callbackQuery.Message.Chat.Id);

                //    return; 
                //}

                //if(callbackQuery.Data == "Category")
                //{
                //    await SendCategoryMenu(callbackQuery.Message.Chat.Id);

                //    return;
                //}

                //if (callbackQuery.Data == "Loreal")
                //{
                //    await SendCategoryMenu(callbackQuery.Message.Chat.Id);
                //    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                //    await _botClient.SendTextMessageAsync(chatId,"Теперь выберите категорию продукта");
                //    return;
                //}
            }

        }





        private Task HandlePollingErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

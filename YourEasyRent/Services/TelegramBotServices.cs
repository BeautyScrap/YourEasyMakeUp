using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace YourEasyRent.Services
{
    public class TelegramBotServices
    {
        private readonly TelegramBotClient _botClient;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public TelegramBotServices(string token) // создаем конструктор для инициализации объектов класса TelegramBotSerices
        {
            _botClient = new TelegramBotClient(token);
        }

        public void StartReceivingMessages()
        {
            var receiverOptions = new ReceiverOptions  //  создаем новый объект  ReceiverOptions для настройки параметров получения обновлений от Telegram API
            {
                AllowedUpdates = Array.Empty<UpdateType>() //  все допустимые обновления будут приниматься в массив UpdateType
            };

            _botClient.StartReceiving // вызываем метод StartReceiving, чтобы начать процесс получения обновлений от Telegram.  В методе StartReceiving определены обработчики обновлений (updateHandler и pollingErrorHandler), опции получения (receiverOptions) и токен отмены (cancellationToken),
                (
                updateHandler: async (bot, update, cancellationToken) => {await HandleUpdateAsync(bot, update, cancellationToken);},

                pollingErrorHandler: async (bot, exception, cancellationToken) => {await HandlePollingErrorAsync(bot, exception, cancellationToken);},

                receiverOptions: receiverOptions,
                cancellationToken: _cts.Token
                );

            var me = _botClient.GetMeAsync().Result;
            Console.WriteLine($"Start listening for @{me.Username}"); // получаем информация о боте с использованием _botClient.GetMeAsync() и выводится его имя пользователя в консоль для отображения информации о начале прослушивания обновлений.

        }

        private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            #region [Main menu]
            InlineKeyboardMarkup mainMenu = new InlineKeyboardMarkup(new[]{new[] {InlineKeyboardButton.WithCallbackData(text:"Оставить заявку", callbackData: "SentOrder")}});

            #endregion
            InlineKeyboardMarkup backMenu = new(new[] { InlineKeyboardButton.WithCallbackData(text:"К главному меню", callbackData: "toBack") });

            if(update.Type == UpdateType.Message &&  update.Message!.Type == MessageType.Text)
            {
                var chatId = update.Message.Chat.Id;
                var messageText = update.Message.Text;
                var firstName = update.Message.From.FirstName;
                Console.WriteLine($"Received a '{messageText}' message in chat {chatId} and user name {firstName}.");
                #region [First Message]
                if (messageText.Contains("/start"))
                {
                     await _botClient.SendTextMessageAsync(chatId, "Приветики! Давай я найду для тебя косметос!", cancellationToken: cancellationToken );
                        return;
                }
                #endregion

                if(update.CallbackQuery != null)
                {
                    if(update.CallbackQuery.Data == "SentOrder")
                    {

                    }

                }
            }





            //var message = update.Message;
            //var chatId = message.Chat.Id;


            //if (message.Text != null)
            //{
            //    Console.WriteLine($"Received a '{message.Text}' message in chat {chatId} and user name {message.Chat.Username}.");

            //    if (message.Text.ToLower().Contains("Привет"))
            //    {
            //        await _botClient.SendTextMessageAsync(message.Chat.Id, "Приветики!", cancellationToken: cancellationToken );
            //        return;
            //    }
            //Console.WriteLine(
            //        $"{message.From.FirstName} sent message {message.MessageId} " +
            //        $"to chat {message.Chat.Id} at {message.Date.ToLocalTime()}. " +
            //        $"It is a reply to message {message.ReplyToMessage.MessageId} " +
            //        $"and has {message.Entities.First().Type == MessageEntityType.Bold} message entities.");

        }
        //// Only process Message updates:
        //if (update.Message is not { } message.)
        //    return; 
        //// Only process text messages
        //if (message.Text is not { } messageText)
        //    return;
        // Echo received message text
        //Message sentMessage =  _botClient.SendTextMessageAsync(chatId: chatId,text: "You said:\n" + messageText,cancellationToken: cancellationToken);
    
       


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
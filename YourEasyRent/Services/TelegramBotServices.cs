﻿using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.VisualBasic;

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
                    await SendMainMenu(chatId);
                    Console.WriteLine($"Received a '{messageText}' message in chat {chatId} and user name {firstName}.");
                    return;
                }
            }
            #endregion
            if (update.Type == UpdateType.CallbackQuery)
            {
                //var chatId = update.Message.Chat.Id;
                //var messageText = update.Message.Text;
                //var firstName = update.Message.From.FirstName;
                //Console.WriteLine($"Received a '{messageText}' message in chat {chatId} and user name {firstName}.");

                var callbackQuery = update.CallbackQuery;

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
            }
        }
        //async Task HandleCallbackQuery(ITelegramBotClient botClient,CallbackQuery callbackQuery )
        //{
        //    if (callbackQuery.Data == "back")
        //    {
        //        await SendMainMenu(callbackQuery.Message.Chat.Id);
        //        await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
        //        return;

        //    }
        //    if (callbackQuery.Data == "Brand")
        //    {
        //        await SendBrandMenu(callbackQuery.Message.Chat.Id);
        //        return;
        //    }

        //}

        async Task SendBrandMenu(long chatId)
        {
            InlineKeyboardMarkup brandMenu = new InlineKeyboardMarkup(new[]
            {new[]
            {InlineKeyboardButton.WithCallbackData(text:"Loreal",callbackData:"Loreal"),
            InlineKeyboardButton.WithCallbackData(text:"MAC",callbackData:"Mac") },

            new[]
            {InlineKeyboardButton.WithCallbackData(text:"Maybelline",callbackData:"Maybelline"),
            InlineKeyboardButton.WithCallbackData(text:"Fenty Beauty",callbackData:"Fenty_Beauty")},
            new[]
            {InlineKeyboardButton.WithCallbackData(text:"Назад",callbackData: "back")}});


            await _botClient.SendTextMessageAsync(chatId, "Выбери бренд", replyMarkup: brandMenu);

        }



        async Task SendMainMenu(long chatId)
        {
           
            InlineKeyboardMarkup mainMenu = new InlineKeyboardMarkup(new[]
            {new[]
            {InlineKeyboardButton.WithCallbackData(text:"Бренд", callbackData: "Brand"),
            InlineKeyboardButton.WithCallbackData(text:"Категория", callbackData: "Category") },
           
            new[]
            {InlineKeyboardButton.WithCallbackData(text:"Назад",callbackData: "back")}});

            await _botClient.SendTextMessageAsync(chatId, "Главное меню", replyMarkup: mainMenu);

        }

        //private static async Task HandleCallbackQuery(ITelegramBotClient botClient, Update update, CallbackQuery callbackQuery)
        //{
        //    if(update.Type == UpdateType.CallbackQuery)

        //    {
        //        var callbackQuery = update.CallbackQuery;

        //    }
        //}



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
﻿using Microsoft.AspNetCore.SignalR;
using System.Numerics;
using Telegram.Bot.Types;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace YourEasyRent.Services
{
    public class TgButtonCallback: ITgButtonCallback
    {
        private readonly Update update;
        public bool IsStart { get; set; }
        public bool IsValidMessage {  get; set; }
        public bool IsValueMenuMessage { get; set; }  
        public bool IsValueProductButton {  get; set; }



        public TgButtonCallback(Update update)
        {
            this.update = update;
        }


        public void IsBotStart()
        {
            var messageText = update.Message?.Text;
            var nameOfButton = update.CallbackQuery?.Data;
            if (messageText != null && messageText.Contains("/start") || nameOfButton == "StartNewSearch") 
            { 
                IsStart = true;
            }
            else
            {
                IsStart = false;
            }
        }
        public long GetUserId(Update update)
        {
            long userId = update.CallbackQuery!.From.Id;
            return userId;
        }
        public long GetChatId(Update update)
        {
            var chatId = update.Message.Chat.Id;
            var userId = update.CallbackQuery!.From.Id;
            if ( chatId  == null)
            {
                chatId = userId;
                return chatId;
            }
            return chatId;
        }
        public void IsValidMsg()
        {
            var nameOfButton = update.CallbackQuery?.Data;
            if (nameOfButton.All(c => char.IsLetter(c) || c == '_'))
            {
                IsValidMessage = true;
            }
            else
            {
                IsValidMessage = false;
                throw new Exception("The user did not send a message");
            }
        }

        public void IsMenuButton()
        {
            var nameOfButton = update.CallbackQuery?.Data;
            if (nameOfButton == "BrandMenu" || nameOfButton == "CategoryMenu")
            {
                IsValueMenuMessage = true;
            }
            else 
            { 
                IsValueMenuMessage = false;
            }
        }
        public void IsProductButton()
        {
            var nameOfButton = update.CallbackQuery?.Data;
            if (nameOfButton.StartsWith("Brand_") || nameOfButton.StartsWith("Category_")) 
            {
                IsValueProductButton = true;
            }
            else
            { 
                IsValueProductButton = false; 
            }
        }

        public string GetNameOfButton(Update update)
        {
            var nameOfButton = update.CallbackQuery?.Data;
            return nameOfButton;
        }        
    }
}

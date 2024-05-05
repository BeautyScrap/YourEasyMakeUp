using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;
using System.Numerics;
using Telegram.Bot.Types;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace YourEasyRent.Services
{
    public class TgButtonCallback
    {
        private readonly Update _update;
        public bool IsStart { get; set; }
        public bool IsValidMessage {  get; set; }
        public bool IsValueMenuMessage { get; set; } 
        public bool IsBrandMenu {  get; set; }  
        public bool IsCategoryMenu { get; set; }    
        public bool IsValueProductButton {  get; set; }


        public TgButtonCallback(Update update)
        {
            _update = update;
        }

        public void IsBotStart()
        {

            var messageText = _update.Message?.Text;
            var nameOfButton = _update.CallbackQuery?.Data;
            if (messageText != null && messageText.Contains("/start") || nameOfButton == "StartNewSearch") 
            { 
                IsStart = true;
            }
            else
            {
                IsStart = false;
            }
        }
        public string GetUserId()
        {
            var userId = _update.CallbackQuery?.From.Id.ToString();
            if (userId == null)
            {
                var startedForUserId = _update.Message?.From.Id.ToString();
                return startedForUserId;
            }
            return userId;
        }
        public string GetChatId()
        {
            var chatId = _update.Message?.Chat.Id.ToString();
            if(chatId == null)
            {
                chatId = _update.CallbackQuery?.From.Id.ToString();
                return chatId;
            }
            return chatId;
        }

        public void IsValidMsg() 
        {
            var nameOfButton = _update.CallbackQuery?.Data;
            if (nameOfButton == null)
            {
                nameOfButton = _update.Message?.Text;

                if (nameOfButton == null)
                {
                    IsValidMessage = false;
                    throw new Exception("The user did not send a message");
                }
            }
            if (nameOfButton.All(c => char.IsLetter(c) || c == '_' || c == '/'))
            {
                IsValidMessage = true;
            }
        }

        public void IsMenuButton()
        {
            var messageText = _update.Message?.Text;
            var nameOfButton = _update.CallbackQuery?.Data;
            if (messageText == null && nameOfButton == "BrandMenu")//  || messageText == null && nameOfButton == "CategoryMenu")
            {
                IsBrandMenu = true;
                IsValueMenuMessage = true;
            }
            if (messageText == null && nameOfButton == "CategoryMenu")
            {
                IsValueMenuMessage = true;
                IsCategoryMenu = true;

            }        
        }

        public string GetMenuButton()
        {
            var menuButton = _update.CallbackQuery.Data;

            if ( menuButton == "BrandMenu")
            {
                return menuButton;
            }
            if (menuButton == "CategoryMenu")
            {
                return menuButton;
            }
            return menuButton;
        }
        public void IsProductButton()
        {
            var messageText = _update.Message?.Text;
            var nameOfButton = _update.CallbackQuery?.Data;
            if (messageText == null && nameOfButton.StartsWith("Brand_") || messageText == null && nameOfButton.StartsWith("Category_")) 
            {
                IsValueProductButton = true;
            } 
        }        
    }
}

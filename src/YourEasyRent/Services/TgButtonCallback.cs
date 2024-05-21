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
        public bool IsStart => IsBotStart();
        public bool IsValidMessage => IsValidMsg();
        public bool IsValueMenuMessage  => IsMenuButton();
        public bool IsBrandMenu => IsBrandMenuButton();
        public bool IsCategoryMenu => IsCategoryMenuButton();
        public bool IsValueProductButton => IsProductButton();
        public bool IsProductBrand=> IsProductBrandButton();
        public bool IsProductCategory=> IsProductCategoryButton();


        public TgButtonCallback(Update update)
        {
            _update = update;
        }

        public bool IsBotStart()
        {
            var messageText = _update.Message?.Text;
            var nameOfButton = _update.CallbackQuery?.Data;
            return messageText != null && messageText.Contains("/start") || nameOfButton == "StartNewSearch";

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

        public bool IsValidMsg()  //  может в меню бота внутрь этого метода поместить все дргие кроме isStart?
        {
            var nameOfButton = _update.CallbackQuery?.Data ?? _update.Message?.Text ?? throw new Exception("The user did not send a message");

            return nameOfButton.All(c => char.IsLetter(c) || c == '_' || c == '/');
            
        }

        public bool IsMenuButton()
        {
            var messageText = _update.Message?.Text;
            var nameOfButton = _update.CallbackQuery?.Data;
            return messageText == null && (nameOfButton == "BrandMenu" || nameOfButton == "CategoryMenu");
        
        }

        public bool IsBrandMenuButton()
        {
            var menuButton = _update.CallbackQuery.Data;
            if (menuButton == "BrandMenu")
            {
                return true;
            }
            return false;
        }

        public bool IsCategoryMenuButton()
        {
            var menuButton = _update.CallbackQuery.Data;
            if (menuButton == "CategoryMenu")
            {
                return true;
            }
            return false;

        }
        //public string GetMenuButton()
        //{
        //    var menuButton = _update.CallbackQuery.Data;

        //    if ( menuButton == "BrandMenu")
        //    {
        //        return menuButton;
        //    }
        //    if (menuButton == "CategoryMenu")
        //    {
        //        return menuButton;
        //    }
        //    return menuButton;
        //}
        public bool IsProductButton()
        {
            var messageText = _update.Message?.Text;
            var nameOfButton = _update.CallbackQuery?.Data;
            return messageText == null && ( nameOfButton.StartsWith("Brand_") || nameOfButton.StartsWith("Category_"));
        }


        public bool IsProductBrandButton()
        {
            var productButton = _update.CallbackQuery.Data;
            if (productButton.StartsWith("Brand_"))
            {
                return true;
            }
            return false;
        }

        public bool IsProductCategoryButton()
        {
            var productButton = _update.CallbackQuery.Data;
            if (productButton.StartsWith("Category_"))
            {
                return true;
            }
            return false;
        }

        public string GetProductButton()
        {

            var productButton = _update.CallbackQuery.Data;
            if (productButton.StartsWith("Brand_"))
            {
                return productButton.Replace("Brand_", "");
            }
            if (productButton.StartsWith("Category_"))
            {
                return productButton.Replace("Category_", "");
            }
            return productButton;

        }
    }
}

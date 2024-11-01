using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace YourEasyRent.TelegramMenu.ButtonHandler
{
    public class CategoryButtonHandler : IButtonHandler

    {
        public CategoryButtonHandler()
        {

        }
        public async Task<InlineKeyboardMarkup> SendMenuToTelegramHandle(string chatId)
        {
            var buttonForCategory = new[]
                {new[]
            {InlineKeyboardButton.WithCallbackData(text:"Mascara",callbackData:"Category_Mascara"),
            InlineKeyboardButton.WithCallbackData(text:"Concealer",callbackData:"Category_Concealer")},
                new[]
            {InlineKeyboardButton.WithCallbackData(text:"Blush",callbackData:"Blush"),
            InlineKeyboardButton.WithCallbackData(text:"Highlighter",callbackData:"Highlighter")},
                new[]
            {InlineKeyboardButton.WithCallbackData(text:"Foundation",callbackData:"Foundation"),
            InlineKeyboardButton.WithCallbackData(text:"Eyeshadow",callbackData:"Eyeshadow")},
                new[]
            {InlineKeyboardButton.WithCallbackData(text:"Brow pencils",callbackData:"Brow pencils"),
            InlineKeyboardButton.WithCallbackData(text:"Lipstick",callbackData:"Lipstick")},
                new[]
            {InlineKeyboardButton.WithCallbackData(text:"Back",callbackData: "Back")}};

            return new InlineKeyboardMarkup(buttonForCategory);
        }
    }
}

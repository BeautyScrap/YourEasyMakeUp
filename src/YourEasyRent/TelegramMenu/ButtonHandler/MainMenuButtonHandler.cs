using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using System.Linq.Expressions;
using YourEasyRent.TelegramMenu.ButtonHandler;

namespace YourEasyRent.Services.Buttons
{
    public class MainMenuButtonHandler : IButtonHandler
    {
        public MainMenuButtonHandler()
        {
        }

        public async Task<InlineKeyboardMarkup> SendMenuToTelegramHandle(string chatId)
        {
            var buttonForMM = new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Brand", callbackData: "BrandMenu"),
                InlineKeyboardButton.WithCallbackData(text: "Product Category", callbackData: "CategoryMenu"),
            },
        };

            return new InlineKeyboardMarkup(buttonForMM);
        }
    }
}
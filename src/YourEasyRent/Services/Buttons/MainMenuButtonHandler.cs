using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using System.Linq.Expressions;

namespace YourEasyRent.Services.Buttons
{
    public class MainMenuButtonHandler : IButtonHandler
    {
        public MainMenuButtonHandler()
        {
        }

        public async Task<InlineKeyboardMarkup> SendMenuToTelegramHandle()
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
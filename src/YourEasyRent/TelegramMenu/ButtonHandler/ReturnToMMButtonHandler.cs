using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using YourEasyRent.TelegramMenu.ButtonHandler;

namespace YourEasyRent.Services.Buttons
{
    public class ReturnToMMButtonHandler : IButtonHandler
    { 

        public ReturnToMMButtonHandler ()
        {
        }
        public  async Task<InlineKeyboardMarkup> SendMenuToTelegramHandle(string chatId)
        {
            var menuaAterSearchResult = new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Start a New Search",callbackData: "StartNewSearch"),
                        InlineKeyboardButton.WithCallbackData(text: "Subscribe to the product",callbackData: "Subscribe"),
                    },
                };

            return new InlineKeyboardMarkup(menuaAterSearchResult);
        }
    }
}

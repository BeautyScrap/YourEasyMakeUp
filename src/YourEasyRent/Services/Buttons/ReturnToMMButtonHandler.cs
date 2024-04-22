using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace YourEasyRent.Services.Buttons
{
    public class ReturnToMMButtonHandler : IButtonHandler
    { 

        public ReturnToMMButtonHandler ()
        {
        }
        public  async Task<InlineKeyboardMarkup> SendMenuToTelegramHandle()
        {
            var menuaAterSearchResult = new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Start a New Search",callbackData: "StartNewSearch"),
                        InlineKeyboardButton.WithCallbackData(text: "Monitor the price",callbackData: "StartNewSearch"),
                    },
                };

            return new InlineKeyboardMarkup(menuaAterSearchResult);
        }
    }
}

using Telegram.Bot.Types.ReplyMarkups;

namespace YourEasyRent.TelegramMenu.ButtonHandler
{
    public class SubscriptionButtonHandler : IButtonHandler

    {
        public  async Task<InlineKeyboardMarkup> SendMenuToTelegramHandle()
        {
            var menuaAterSubscription = new[]
    {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Start a New Search",callbackData: "StartNewSearch")
                    },
                };

            return new InlineKeyboardMarkup(menuaAterSubscription);
        }
    }
}

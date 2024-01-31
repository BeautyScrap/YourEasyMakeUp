using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace YourEasyRent.Services.Buttons
{
    public class MainMenuButtonHandler : IButtonHandler
    {
        private readonly ITelegramBotClient _botClient;
        public MainMenuButtonHandler(ITelegramBotClient botClient)
        {

            _botClient = botClient;
        }

        public async Task Handle(long chatId)
        {
            var menu = new InlineKeyboardMarkup(
                new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Brand",callbackData: "BrandMenu"),
                        InlineKeyboardButton.WithCallbackData("Product Category",callbackData: "CategoryMenu"),
                    },
                });

            await SendMeinMenuInlineKeyboardButton(chatId, menu);
        }

        private async Task<Message> SendMeinMenuInlineKeyboardButton(long chatId, InlineKeyboardMarkup telegramMenu)
        {
            await _botClient.SendTextMessageAsync(chatId, "Hello! Let me find some cosmetics for you!");
            await _botClient.SendTextMessageAsync(chatId, "Main menu", replyMarkup: telegramMenu);

            return await _botClient.SendTextMessageAsync(chatId, text: "Choose");

        }
    }
}
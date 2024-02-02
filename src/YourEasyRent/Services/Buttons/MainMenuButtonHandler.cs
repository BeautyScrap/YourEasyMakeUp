using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using System.Linq.Expressions;

namespace YourEasyRent.Services.Buttons
{
    public class MainMenuButtonHandler : IButtonHandler
    {
        private readonly ITelegramBotClient _botClient;
        public MainMenuButtonHandler(ITelegramBotClient botClient)
        {

            _botClient = botClient;
        }

        public async Task SendMenuToTelegramHandle(long chatId)
        {
            var menu = new InlineKeyboardMarkup(
                new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Brand",callbackData: "BrandMenu"),
                        InlineKeyboardButton.WithCallbackData(text: "Product Category",callbackData: "CategoryMenu"),
                    },
                });

            await SendMeinMenuInlineKeyboardButton(chatId, menu);
        }

        private async Task<Message> SendMeinMenuInlineKeyboardButton(long chatId, InlineKeyboardMarkup telegramMenu)
        {
            try
            {
                var test = await _botClient.SendTextMessageAsync(chatId, "Main menu", replyMarkup: telegramMenu);
            }
            catch (Exception e)
            {
                var zalupa = e;

            }
            await _botClient.SendTextMessageAsync(chatId, "Hello! Let me find some cosmetics for you!");
            return await _botClient.SendTextMessageAsync(chatId, text: "Choose");
        }
    }
}
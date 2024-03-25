using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace YourEasyRent.Services.Buttons
{
    public class ReturnToMMButtonHandler : IButtonHandler
    {
        private readonly ITelegramBotClient _botClient;

        public ReturnToMMButtonHandler (ITelegramBotClient botClient)
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
                        InlineKeyboardButton.WithCallbackData(text: "Start a New Search",callbackData: "StartNewSearch"),
                    },
                });

            await SendMeinMenuInlineKeyboardButton(chatId, menu);
        }

        private  async Task<Message> SendMeinMenuInlineKeyboardButton(long chatId, InlineKeyboardMarkup menu)
        {
            return await _botClient.SendTextMessageAsync(chatId, "Do you want to start a new search?", replyMarkup: menu);
        }
    }
}

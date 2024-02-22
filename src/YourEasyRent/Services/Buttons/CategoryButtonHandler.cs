using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace YourEasyRent.Services.Buttons
{
    public class CategoryButtonHandler : IButtonHandler

    {
        private readonly ITelegramBotClient _botClient;

        public CategoryButtonHandler(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }
        public async Task SendMenuToTelegramHandle(long chatId)
        {
            var menu = new InlineKeyboardMarkup(
                new[]
                {
                    new[]
            {InlineKeyboardButton.WithCallbackData(text:"Mascara",callbackData:"Category_Mascara"),
            InlineKeyboardButton.WithCallbackData(text:"Concealer",callbackData:"Category_Concealer")}});
            //new[]
            //{InlineKeyboardButton.WithCallbackData(text:"Blush",callbackData:"Blush"),
            //InlineKeyboardButton.WithCallbackData(text:"Highlighter",callbackData:"Highlighter")},
            //new[]
            //{InlineKeyboardButton.WithCallbackData(text:"Foundation",callbackData:"Foundation"),
            //InlineKeyboardButton.WithCallbackData(text:"Eyeshadow",callbackData:"Eyeshadow")},
            //new[]
            //{InlineKeyboardButton.WithCallbackData(text:"Brow pencils",callbackData:"Brow pencils"),
            //InlineKeyboardButton.WithCallbackData(text:"Lipstick",callbackData:"Lipstick")},
            //new[]
            //{InlineKeyboardButton.WithCallbackData(text:"Back",callbackData: "Back")}});
            await SendCategotyInlineeyboardMarkup(menu , chatId);   
        }

        private async Task<Message> SendCategotyInlineeyboardMarkup(InlineKeyboardMarkup menu, long chatId)
        {
            return await _botClient.SendTextMessageAsync(chatId, "Сhoose the category:", replyMarkup: menu);
        }
    }
}

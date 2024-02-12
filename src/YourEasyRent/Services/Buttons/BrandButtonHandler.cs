using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace YourEasyRent.Services.Buttons
{
    internal class BrandButtonHandler : IButtonHandler
    {
        private readonly ITelegramBotClient _botClient;
        public BrandButtonHandler(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task SendMenuToTelegramHandle(long chatId)
        {
            var menu =  new InlineKeyboardMarkup(
                new[]
                {
                    new[]
            {InlineKeyboardButton.WithCallbackData(text:"TARTE",callbackData:"TARTE"),// написать префикс brand_Tarte
            InlineKeyboardButton.WithCallbackData(text:"MAC",callbackData:"MAC") },
                    new[]
            {InlineKeyboardButton.WithCallbackData(text:"Maybelline",callbackData:"Brand_Maybelline"),
            InlineKeyboardButton.WithCallbackData(text:"FENTY BEAUTY",callbackData:"FENTY_BEAUTY")},
                    new[]
            {InlineKeyboardButton.WithCallbackData(text:"Back",callbackData: "Back")}});

            await SendBrandMenuInlineKeyboardButton(chatId, menu);
        }

        private async Task<Message> SendBrandMenuInlineKeyboardButton(long chatId, InlineKeyboardMarkup menu)
        {
            return await _botClient.SendTextMessageAsync(chatId, "Сhoose the brand:", replyMarkup: menu);
        }
    }
}
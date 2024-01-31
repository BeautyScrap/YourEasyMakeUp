using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.Mime.MediaTypeNames;

namespace YourEasyRent.Services.Buttons
{
    internal class BrandButtonHandler : IButtonHandler
    {
        private readonly ITelegramBotClient _botClient;
        public BrandButtonHandler(ITelegramBotClient botClient)
        {

            _botClient = botClient;
        }

        public async Task Handle(long chatId)
        {
            var menu =  new InlineKeyboardMarkup(
                new[]
                {
                    new[]
            {InlineKeyboardButton.WithCallbackData(text:"TARTE",callbackData:"TARTE"),
            InlineKeyboardButton.WithCallbackData(text:"MAC",callbackData:"MAC") },
                    new[]
            {InlineKeyboardButton.WithCallbackData(text:"Maybelline",callbackData:"Maybelline"),
            InlineKeyboardButton.WithCallbackData(text:"FENTY BEAUTY",callbackData:"FENTY_BEAUTY")},
                    new[]
            {InlineKeyboardButton.WithCallbackData(text:"Back",callbackData: "Back")}});

            await SendBrandMenuInlineKeyboardButton(chatId, menu);
        }

        private async Task<Message> SendBrandMenuInlineKeyboardButton(long chatId, InlineKeyboardMarkup telegramMenu)
        {
            //await _botClient.SendTextMessageAsync(chatId, "Сhoose the brand:", replyMarkup: telegramMenu);
            return await _botClient.SendTextMessageAsync(chatId, "Сhoose the brand:", replyMarkup: telegramMenu);


        }
    }
}
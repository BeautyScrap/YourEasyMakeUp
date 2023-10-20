using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
namespace YourEasyRent.Services
{
    public class TelegramMenu : ITelegramMenu
    {

        public TelegramMenu() { }

        public InlineKeyboardMarkup meinMenu =>
            new (new[]
            {new[]
            {InlineKeyboardButton.WithCallbackData(text:"Brand", callbackData: "Brand"),
            InlineKeyboardButton.WithCallbackData(text:"Product Category", callbackData: "Category") },
            new[]
            {InlineKeyboardButton.WithCallbackData(text:"Price", callbackData:"Price"),
            InlineKeyboardButton.WithCallbackData(text:"Back ",callbackData: "Back")}});

        

        public InlineKeyboardMarkup brandMenu =>
            new (new[]
            {new[]
            {InlineKeyboardButton.WithCallbackData(text:"Loreal",callbackData:"Loreal"),
            InlineKeyboardButton.WithCallbackData(text:"MAC",callbackData:"MAC") },

            new[]
            {InlineKeyboardButton.WithCallbackData(text:"Maybelline",callbackData:"Maybelline"),
            InlineKeyboardButton.WithCallbackData(text:"FENTY BEAUTY",callbackData:"FENTY_BEAUTY")},
            new[]
            {InlineKeyboardButton.WithCallbackData(text:"Back",callbackData: "Back")}});

        public InlineKeyboardMarkup categoryMenu =>
            new (new[]
            {new[]
            {InlineKeyboardButton.WithCallbackData(text:"Foundation",callbackData:"Foundation"),
            InlineKeyboardButton.WithCallbackData(text:"Consealer",callbackData:"Consealer"),},

            new[]
            {InlineKeyboardButton.WithCallbackData(text:"Blush",callbackData:"Blush"),
            InlineKeyboardButton.WithCallbackData(text:"Highlighter",callbackData:"Highlighter")},

            new[]
            {InlineKeyboardButton.WithCallbackData(text:"Mascara",callbackData:"Mascara"),
            InlineKeyboardButton.WithCallbackData(text:"Eyeshadow",callbackData:"Eyeshadow")},

            new[]
            {InlineKeyboardButton.WithCallbackData(text:"Brow pencils",callbackData:"Brow pencils"),
            InlineKeyboardButton.WithCallbackData(text:"Lipstick",callbackData:"Lipstick")},

            new[]
            {InlineKeyboardButton.WithCallbackData(text:"Back",callbackData: "Back")}});

        public InlineKeyboardMarkup priceMenu =>
            new (new[]
            {new[]
            {InlineKeyboardButton.WithCallbackData(text:"from 1 to 5",callbackData:"from_1_to_5"),
            InlineKeyboardButton.WithCallbackData(text:"from 6 to 10",callbackData:"from_6_to_10") },

            new[]
            {InlineKeyboardButton.WithCallbackData(text:"from 11 to 25",callbackData:"from_11_to_25"),
            InlineKeyboardButton.WithCallbackData(text:"from 26 to 50",callbackData:"from_26_to_50")},
            new[]
            {InlineKeyboardButton.WithCallbackData(text:"Back",callbackData: "Back")}});
    }
}

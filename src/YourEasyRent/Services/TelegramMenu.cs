using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
namespace YourEasyRent.Services
{
    public class TelegramMenu : ITelegramMenu
    {
        public TelegramMenu() { }

        public InlineKeyboardMarkup CallMeinMenu()
        {
            return new InlineKeyboardMarkup(
                new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Brand",callbackData: "BrandMenu"),
                        InlineKeyboardButton.WithCallbackData("Product Category",callbackData: "CategoryMenu"),
                    },
                });
        }

        public InlineKeyboardMarkup CallBrandMenu()
        {
            return new InlineKeyboardMarkup(
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
        }
        public InlineKeyboardMarkup CallCategoryMenu()
        {
            return new InlineKeyboardMarkup(
                new[]
                {
                    new[]
            {InlineKeyboardButton.WithCallbackData(text:"Foundation",callbackData:"Foundation"),
            InlineKeyboardButton.WithCallbackData(text:"Concealer",callbackData:"Concealer"),},
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
        }
        public InlineKeyboardMarkup CallReturnToMeinMenu()
        {
            return new InlineKeyboardMarkup(
                new[]
            {new[]
            {InlineKeyboardButton.WithCallbackData(text:"Return To Mein Menu",callbackData:"Return_To_MeinMenu")}});     
        }
    }
}

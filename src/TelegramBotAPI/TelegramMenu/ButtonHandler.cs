using Telegram.Bot.Types.ReplyMarkups;

namespace YourEasyRent.TelegramMenu
{
    public class ButtonHandler
    {
        public ButtonHandler()
        {
        }
        public async Task<InlineKeyboardMarkup> SendMainMenuKeyboard(string chatId)
        {
            var buttonForMM = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Brand", callbackData: "BrandMenu"),
                    InlineKeyboardButton.WithCallbackData(text: "Product Category", callbackData: "CategoryMenu"),
                },
            };
            return new InlineKeyboardMarkup(buttonForMM);
        }

        public async Task<InlineKeyboardMarkup> SendCategoryMenuKeyboard(string chatId)
        {
            var buttonForCategory = new[]
            {
                new[]
            {InlineKeyboardButton.WithCallbackData(text:"Mascara",callbackData:"Category_Mascara"),
            InlineKeyboardButton.WithCallbackData(text:"Concealer",callbackData:"Category_Concealer")},
                new[]
            {InlineKeyboardButton.WithCallbackData(text:"Blush",callbackData:"Blush"),
            InlineKeyboardButton.WithCallbackData(text:"Highlighter",callbackData:"Highlighter")},
                new[]
            {InlineKeyboardButton.WithCallbackData(text:"Foundation",callbackData:"Foundation"),
            InlineKeyboardButton.WithCallbackData(text:"Eyeshadow",callbackData:"Eyeshadow")},
                new[]
            {InlineKeyboardButton.WithCallbackData(text:"Brow pencils",callbackData:"Brow pencils"),
            InlineKeyboardButton.WithCallbackData(text:"Lipstick",callbackData:"Lipstick")},
                new[]
            {InlineKeyboardButton.WithCallbackData(text:"Back",callbackData: "Back")}};
            return new InlineKeyboardMarkup(buttonForCategory);
        }

        public async Task<InlineKeyboardMarkup> SendBrandMenuKeyboard(string chatId, List<string> brands)
        {
            var InlineKeyboardMarkup = CreateBrandInlineKeyboardMarkup(brands);
            return InlineKeyboardMarkup;
        }

        public InlineKeyboardMarkup CreateBrandInlineKeyboardMarkup(List<string> brands)
        {
            var InlineKeyboardButtons = brands.Select(brand =>
            {
                var buttone = InlineKeyboardButton.WithCallbackData(text: brand, callbackData: $"Brand_{brand}");
                return new List<InlineKeyboardButton> { buttone };
            }).ToList();
            return new InlineKeyboardMarkup(InlineKeyboardButtons);
        }
        public async Task<InlineKeyboardMarkup> SendReturnToMainMenuKeyboard(string chatId)
        {
            var menuaAterSearchResult = new[]
            {
                new []
                {
                     InlineKeyboardButton.WithCallbackData(text: "Start a New Search",callbackData: "StartNewSearch"),
                     InlineKeyboardButton.WithCallbackData(text: "Subscribe to the product",callbackData: "Subscribe"),
                },
            };
            return new InlineKeyboardMarkup(menuaAterSearchResult);
        }
        public async Task<InlineKeyboardMarkup> SendNewSearchKeyboard(string chatId)
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
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotAPI.Application.TelegramMenu
{
    public class ButtonHandler
    {
        public ButtonHandler()
        {
        }

        public InlineKeyboardMarkup CreateMainMenuKeyboard()
        {
            var mainMenuButtons = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Brand", callbackData: "BrandMenu"),
                    InlineKeyboardButton.WithCallbackData(text: "Product Category", callbackData: "CategoryMenu"),
                },
            };
            return  new InlineKeyboardMarkup(mainMenuButtons);
        }

        public InlineKeyboardMarkup CreateCategoryMenuKeyboard()
        {
            var categoryButtons = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Mascara", callbackData: "Category_Mascara"),
                    InlineKeyboardButton.WithCallbackData(text: "Concealer", callbackData: "Category_Concealer")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Blush", callbackData: "Category_Blush"),
                    InlineKeyboardButton.WithCallbackData(text: "Highlighter", callbackData: "Category_Highlighter")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Foundation", callbackData: "Category_Foundation"),
                    InlineKeyboardButton.WithCallbackData(text: "Eyeshadow", callbackData: "Category_Eyeshadow")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Brow pencils", callbackData: "Category_BrowPencils"),
                    InlineKeyboardButton.WithCallbackData(text: "Lipstick", callbackData: "Category_Lipstick")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Back", callbackData: "Back")
                }
            };
            return new InlineKeyboardMarkup(categoryButtons);
        }

        public InlineKeyboardMarkup CreateBrandMenuKeyboard(List<string> brands)
        {
            var brandButtons = brands.Select(brand =>
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: brand, callbackData: $"Brand_{brand}")
                }).ToList();

            return new InlineKeyboardMarkup(brandButtons);
        }

        public InlineKeyboardMarkup CreateSearchResultMenuKeyboard()
        {
            var searchResultButtons = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Start a New Search", callbackData: "StartNewSearch"),
                    InlineKeyboardButton.WithCallbackData(text: "Subscribe to the product", callbackData: "Subscribe"),
                },
            };
            return new InlineKeyboardMarkup(searchResultButtons);
        }

        public InlineKeyboardMarkup CreateNewSearchKeyboard()
        {
            var newSearchButton = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Start a New Search", callbackData: "StartNewSearch")
                },
            };
            return new InlineKeyboardMarkup(newSearchButton);
        }
    }
}
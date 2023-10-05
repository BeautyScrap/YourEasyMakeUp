using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace YourEasyRent.Services;

// It is easier if service is small and does only one thing
public class TelegramMenu : ITelegramMenu
{
    public TelegramMenu()
    {
    }

    public InlineKeyboardMarkup BrandMenu =>
        new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Loreal", callbackData: "Loreal"),
                InlineKeyboardButton.WithCallbackData(text: "MAC", callbackData: "Mac")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Maybelline", callbackData: "Maybelline"),
                InlineKeyboardButton.WithCallbackData(text: "Fenty Beauty", callbackData: "Fenty_Beauty")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "back")
            }
        });

    public InlineKeyboardMarkup CategoryMenu =>
        new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Foundation", callbackData: "Foundation"),
                InlineKeyboardButton.WithCallbackData(text: "Consealer", callbackData: "Consealer"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Blush", callbackData: "Blush"),
                InlineKeyboardButton.WithCallbackData(text: "Highlighter", callbackData: "Highlighter")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Mascara", callbackData: "Mascara"),
                InlineKeyboardButton.WithCallbackData(text: "Eyeshadow", callbackData: "Eyeshadow")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Brow pencils", callbackData: "Brow pencils"),
                InlineKeyboardButton.WithCallbackData(text: "Lipstick", callbackData: "Lipstick")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "back", callbackData: "back")
            }
        });

    public InlineKeyboardMarkup MainMenu =>
        new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Бренд", callbackData: "Brand"),
                InlineKeyboardButton.WithCallbackData(text: "Категория", callbackData: "Category")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "back")
            }
        });
}

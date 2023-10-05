using Telegram.Bot.Types.ReplyMarkups;

namespace YourEasyRent.Services;

public interface ITelegramMenu
{
    InlineKeyboardMarkup BrandMenu { get; }
    InlineKeyboardMarkup CategoryMenu { get; }
    InlineKeyboardMarkup MainMenu { get; }
}

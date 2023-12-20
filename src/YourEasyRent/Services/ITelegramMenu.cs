using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace YourEasyRent.Services
{
    public interface ITelegramMenu
    {
        InlineKeyboardMarkup Mein { get; }
        InlineKeyboardMarkup Brand { get; }
        InlineKeyboardMarkup Category { get; }
        InlineKeyboardMarkup ReturnToMeinMenu {  get; }

    }
}

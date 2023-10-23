using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace YourEasyRent.Services
{
    public interface ITelegramMenu
    {
        InlineKeyboardMarkup meinMenu { get; }
        InlineKeyboardMarkup brandMenu { get; }
        InlineKeyboardMarkup categoryMenu { get; }
        InlineKeyboardMarkup ReturnToMeinMenu {  get; }

    }
}

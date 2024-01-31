using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace YourEasyRent.Services
{
    public interface ITelegramMenu
    {
        InlineKeyboardMarkup CallMeinMenu();
        InlineKeyboardMarkup CallBrandMenu();
        InlineKeyboardMarkup CallCategoryMenu();
        InlineKeyboardMarkup CallReturnToMeinMenu();

    }
}

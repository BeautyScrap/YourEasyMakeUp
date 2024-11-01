using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using YourEasyRent.Entities;

namespace YourEasyRent.TelegramMenu.ButtonHandler
{
    internal interface IButtonHandler
    {
        Task<InlineKeyboardMarkup> SendMenuToTelegramHandle(string chatId);
    }
}
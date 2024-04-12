using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using YourEasyRent.Entities;

namespace YourEasyRent.Services.Buttons
{
    internal interface IButtonHandler
    {
       Task<InlineKeyboardMarkup> SendMenuToTelegramHandle();
    }
}
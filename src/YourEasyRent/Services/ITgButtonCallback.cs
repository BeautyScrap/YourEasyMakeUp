using Telegram.Bot.Types;

namespace YourEasyRent.Services
{
    public interface ITgButtonCallback
    {
        bool IsStart { get; set; }
        bool IsValidMessage { get;  set; }
        bool IsValueMenuMessage { get;set; }
        bool IsValueProductButton { get;  set; }
        void IsBotStart();
        long GetUserId(Update update);
        long GetChatId(Update update);  
        void IsValidMsg();
        void IsMenuButton(); // categoryMenu, brandMenu
        void IsProductButton(); // sephora, dildo, elf  -куда мы идем, чтобы проверить валидность кнопки?проверка на знаки и буквы?
        string GetNameOfButton(Update update);


    }
}

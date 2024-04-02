using Telegram.Bot.Types;

namespace YourEasyRent.Services;

public interface ITelegramCallbackHandler
{   
    Task HandleUpdateAsync(Update update);
}


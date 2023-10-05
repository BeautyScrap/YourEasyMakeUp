using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

using YourEasyRent.DataBase.Interfaces;

namespace YourEasyRent.Services;

public class TelegramActionsHandler : ITelegramActionsHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IProductRepository _productRepository;

    public TelegramActionsHandler(ITelegramBotClient telegramClient, IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task ShowFilteredProducts(long chatId, string category, string brand)
    {
        throw new NotImplementedException();
    }
}

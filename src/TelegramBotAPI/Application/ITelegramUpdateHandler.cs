using Microsoft.AspNetCore.Mvc;
using YourEasyRent.Entities.ProductForSubscription;

namespace TelegramBotAPI.Services
{
    public interface ITelegramUpdateHandler
    {
        Task HandlerUpdateAsync(ProductForSubscription newProduct);
    }
}

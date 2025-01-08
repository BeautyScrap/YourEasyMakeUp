using Microsoft.AspNetCore.Mvc;
using YourEasyRent.Entities.ProductForSubscription;
using YourEasyRent.TelegramMenu;

namespace TelegramBotAPI.Services
{
    public class TelegramUpdateHandler : ITelegramUpdateHandler
    {
        private readonly ITelegramSender _telegramSender;
        public TelegramUpdateHandler(ITelegramSender telegramSender)
        {
            _telegramSender = telegramSender;
        }
        public async Task HandlerUpdateAsync(ProductForSubscription newProduct)
        {
            var chatId = newProduct.ChatId;
            await _telegramSender.SendSubscriberProduct(chatId, newProduct); 
        }
    }
}

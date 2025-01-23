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
        public async Task HandlerUpdateAsync(ProductForSubscription newProduct)// AK TODO вопрос: это метод что-то должен вернуть?
        {
            var userId = newProduct.UserId;
            await _telegramSender.SendSubscriberProduct(userId, newProduct); 
        }
    }
}

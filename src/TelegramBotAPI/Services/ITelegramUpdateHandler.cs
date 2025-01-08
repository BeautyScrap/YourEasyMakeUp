using Microsoft.AspNetCore.Mvc;
using YourEasyRent.Entities.ProductForSubscription;

namespace TelegramBotAPI.Services
{
    public interface ITelegramUpdateHandler
    {
        Task HandlerUpdateAsync(ProductForSubscription newProduct); // AK TODO  !lastUpdate  остановилась тута,надо дописать этот метод,
                                                                                                 // который будет принимать в себя обновленный продукт и пересылать его пользаку и после отправки пользаку присылать ответ "ок, сообщение отправлено" или код
    }
}

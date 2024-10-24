using YourEasyRent.Entities;
using YourEasyRent.Entities.ProductForSubscription;

namespace YourEasyRent.TelegramMenu
{
    public interface ITelegramSender
    {
        Task SendMainMenu(string chatId);
        Task SendCategoryMenu(string chatId);
        Task SendBrandMenu(string chatId);
        Task<IEnumerable<string>> SendResults(string chatId, List<string> listWithResult);
        Task SendMenuAfterResult(string chatId);
        Task SendConfirmOfSubscriprion(string chatId);
        Task SendSubscriberProduct(string chatId, ProductForSubscription product);
        
    }
}

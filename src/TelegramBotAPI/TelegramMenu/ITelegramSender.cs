using YourEasyRent.Entities;
using YourEasyRent.Entities.ProductForSubscription;

namespace YourEasyRent.TelegramMenu
{
    public interface ITelegramSender
    {
        Task SendMainMenu(string chatId);
        Task SendCategoryMenu(string chatId);
        Task SendBrandMenu(string chatId, List<string> brands);
        Task SendMenuAfterResult(string chatId);
        Task SendConfirmOfSubscriprion(string chatId);
        Task SendSubscriberProduct(string chatId, ProductForSubscription product);
        Task SendOneResult(string chatId, string resultOfSearch);

    }
}

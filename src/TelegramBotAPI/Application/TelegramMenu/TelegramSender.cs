using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using YourEasyRent.Entities;
using YourEasyRent.Entities.ProductForSubscription;


namespace TelegramBotAPI.Application.TelegramMenu
{
    public class TelegramSender : ITelegramSender
    {
        private readonly ITelegramBotClient _botClient;

        public TelegramSender(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }
        public async Task SendMainMenu(string chatId)
        {
            var menu = new ButtonHandler().CreateMainMenuKeyboard();
            await _botClient.SendMessage(chatId, "Main menu. Choose one:", replyMarkup: menu);
        }
        public async Task SendBrandMenu(string chatId, List<string> brands)
        {
            var menu = new ButtonHandler().CreateBrandMenuKeyboard(brands);
            await _botClient.SendMessage(chatId, "Сhoose a brand:", replyMarkup: menu);
        }

        public async Task SendCategoryMenu(string chatId)
        {
            var menu = new ButtonHandler().CreateCategoryMenuKeyboard();
            await _botClient.SendMessage(chatId, "Сhoose a category:", replyMarkup: menu);
        }

        public async Task SendMenuAfterResult(string chatId)
        {
            var menu = new ButtonHandler().CreateSearchResultMenuKeyboard();
            await _botClient.SendMessage(chatId, "What do you want to do next?", replyMarkup: menu);
        }

        public async Task SendConfirmOfSubscriprion(string chatId)
        {
            await _botClient.SendMessage(chatId, "The Product saved!", parseMode: ParseMode.Markdown);
            var menu = new ButtonHandler().CreateNewSearchKeyboard();
            await _botClient.SendMessage(chatId, null, replyMarkup: menu);
        }

        public async Task SendSubscriberProduct(string userId, ProductForSubscription product)
        {
            var result = product.ToString();
            await _botClient.SendMessage(userId, $"WOW!The price of your product has become lower!  {result}", parseMode: ParseMode.Markdown);
        }


        public async Task SendOneResult(string chatId, string resultOfSearch)
        {
            await _botClient.SendMessage(chatId, resultOfSearch, parseMode: ParseMode.Markdown);
        }

    }

}

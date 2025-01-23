using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using YourEasyRent.Entities;
using YourEasyRent.Entities.ProductForSubscription;


namespace YourEasyRent.TelegramMenu
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
            var menu = await new ButtonHandler().SendMainMenuKeyboard(chatId);
            await _botClient.SendTextMessageAsync(chatId, "Main menu. Choose one:", replyMarkup: menu);
        }
        public async Task SendBrandMenu(string chatId, List<string> brands) 
        {
            var menu = await new ButtonHandler().SendBrandMenuKeyboard(chatId, brands);
            await _botClient.SendTextMessageAsync(chatId, "Сhoose a brand:", replyMarkup: menu);
        }

        public async Task SendCategoryMenu(string chatId)
        {
            var menu = await new ButtonHandler().SendCategoryMenuKeyboard(chatId);
            await _botClient.SendTextMessageAsync(chatId, "Сhoose a category:", replyMarkup: menu);
        }

        public async Task SendMenuAfterResult(string chatId) 
        {
            var menu = await new ButtonHandler().SendReturnToMainMenuKeyboard(chatId);
            await _botClient.SendTextMessageAsync(chatId, "What do you want to do next?", replyMarkup: menu);
        }

        public async Task SendConfirmOfSubscriprion(string chatId)
        {
           
            await _botClient.SendTextMessageAsync(chatId, "The Product saved!", parseMode: ParseMode.Markdown);
            var menu = await new ButtonHandler().SendNewSearchKeyboard(chatId);
            await _botClient.SendTextMessageAsync(chatId, null, replyMarkup: menu);
        }

        public async Task SendSubscriberProduct(string userId, ProductForSubscription product)
        {
            var result = product.ToString(); 
            await _botClient.SendTextMessageAsync(userId, $"WOW!The price of your product has become lower!  {result}", parseMode: ParseMode.Markdown);      
        }


        public async Task SendOneResult(string chatId, string resultOfSearch)
        {
            await _botClient.SendTextMessageAsync(chatId, resultOfSearch, parseMode: ParseMode.Markdown);
        }

    }
    
}

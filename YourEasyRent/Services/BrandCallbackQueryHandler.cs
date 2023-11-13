using Telegram.Bot;
using Telegram.Bot.Types;
using YourEasyRent.Entities;

namespace YourEasyRent.Services
{
    public class BrandCallbackQueryHandler
    {
        private readonly TelegramMenu _telegramMenu;
        private readonly TelegramBotClient _botClient;
        private string _currentBrand;
        private BotState _currentBotState;

        public BrandCallbackQueryHandler(TelegramMenu telegramMenu, TelegramBotClient botClient)
        {
            _telegramMenu = telegramMenu;
            _botClient = botClient;
        }

        public async Task AnswerBrandCallbackQuery(CallbackQuery callbackQuery, string brand, long chatId, string firstName)
        {
            _currentBrand = brand;
            _currentBotState = BotState.CategorySelected;
            Console.WriteLine($"Received a '{callbackQuery!.Data}' message in chat {chatId} and user name {firstName}.");
            await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
            await _botClient.SendTextMessageAsync(callbackQuery.Id, "Now choose the category of the product", replyMarkup: _telegramMenu.Category);
        }

    }
}

using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using YourEasyRent.Entities;
using System.Threading.Tasks;


namespace YourEasyRent.Services
{
    public class BrandCallbackQueryHandler
    {
        private readonly ITelegramBotClient _botClient;      
        private readonly ITelegramMenu _telegramMenu;
        private string _currentBrand;
        private BotState _currentBotState;

        public BrandCallbackQueryHandler(ITelegramBotClient botClient,ITelegramMenu telegramMenu)
        {
            _telegramMenu = telegramMenu;
            _botClient = botClient;
        }

        public async Task AnswerBrandCallbackQuery(CallbackQuery callbackQuery, string brand, long chatId, string firstName)
        {
            _currentBrand = brand;
           // _currentBotState = BotState.CategorySelected;
            Console.WriteLine($"Received a '{callbackQuery!.Data}' message in chat {chatId} and user name {firstName}.");
            //await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
            await _botClient.SendTextMessageAsync(chatId, "Now choose the category of the product", replyMarkup: _telegramMenu.Category);
        }
        
    }
}

using Microsoft.OpenApi.Extensions;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Entities;

namespace YourEasyRent.Services.Buttons
{
    internal class BrandButtonHandler : IButtonHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IProductRepository _productRepository;


        public BrandButtonHandler(ITelegramBotClient botClient,IProductRepository productRepository)
        {
            _botClient = botClient;
            _productRepository = productRepository; 
        }


        public async Task SendMenuToTelegramHandle(long chatId)
        {
            var brandsMenu = await _productRepository.GetBrandForMenu(limit: 5);
            var InlineKeyboardMarkup = CreateInlineKeyboardMarkup(brandsMenu);

            await SendBrandMenuInlineKeyboardButton(chatId, InlineKeyboardMarkup);
        }

        private InlineKeyboardMarkup CreateInlineKeyboardMarkup(List<string> brandsMenu)
        {
            var InlineKeyboardButtons = new List<List<InlineKeyboardButton>>();
            foreach (var brand in brandsMenu)
            {
                var buttone = InlineKeyboardButton.WithCallbackData(text: brand, callbackData: brand);
                InlineKeyboardButtons.Add(new List<InlineKeyboardButton> { buttone });
            }
            return new InlineKeyboardMarkup(InlineKeyboardButtons);
        }

    
        private async Task<Message> SendBrandMenuInlineKeyboardButton(long chatId, InlineKeyboardMarkup brandsMenu)
        {
            return await _botClient.SendTextMessageAsync(chatId, "Сhoose the brand:", replyMarkup: brandsMenu);
        }   
    }
}
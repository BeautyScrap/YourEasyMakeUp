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
        private readonly IProductRepository _productRepository;

        public BrandButtonHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository; 
        }

        public  async Task<InlineKeyboardMarkup> SendMenuToTelegramHandle()
        {
            var brandsMenu = await _productRepository.GetBrandForMenu(limit: 5);
            var InlineKeyboardMarkup = CreateInlineKeyboardMarkup(brandsMenu);
            return InlineKeyboardMarkup;

        }

        private InlineKeyboardMarkup CreateInlineKeyboardMarkup(List<string> brandsMenu)
        {
            var InlineKeyboardButtons = brandsMenu.Select(brand =>
            {
                var buttone = InlineKeyboardButton.WithCallbackData(text: brand, callbackData: brand);
                return new List<InlineKeyboardButton> { buttone };
            }).ToList();
           
            return new InlineKeyboardMarkup(InlineKeyboardButtons);

        }   
    }
}
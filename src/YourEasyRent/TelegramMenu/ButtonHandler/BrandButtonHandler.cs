using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotAPI.Services;


namespace YourEasyRent.TelegramMenu.ButtonHandler
{
    internal class BrandButtonHandler : IButtonHandler
    {
       // AK TODO - наверно неправильно отправлять запрос в другой микросервис прямо от сюда?
        private readonly IProductApiClient _client;                                                       // надо для него тогда сделать новый контроллер?

        public BrandButtonHandler(IProductApiClient client)
        {
            _client = client;
        }

        public async Task<InlineKeyboardMarkup> SendMenuToTelegramHandle(string chatId)
        {
            var brands = await _client.GetBrandForMenu(chatId, limit: 5);
            //var brandsMenu = await _productRepository.GetBrandForMenu(limit: 5);
            var InlineKeyboardMarkup = CreateInlineKeyboardMarkup(brands);
            return InlineKeyboardMarkup;

        }

        private InlineKeyboardMarkup CreateInlineKeyboardMarkup(List<string> brands)
        {
            var InlineKeyboardButtons = brands.Select(brand =>
            {
                var buttone = InlineKeyboardButton.WithCallbackData(text: brand, callbackData: $"Brand_{brand}");
                return new List<InlineKeyboardButton> { buttone };
            }).ToList();

            return new InlineKeyboardMarkup(InlineKeyboardButtons);

        }
    }
}
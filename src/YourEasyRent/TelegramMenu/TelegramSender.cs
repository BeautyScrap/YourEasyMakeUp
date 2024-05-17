using Amazon.Runtime.Internal.Transform;
using System.Security.Cryptography.X509Certificates;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using YourEasyRent.DataBase;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Entities;
using YourEasyRent.Services.Buttons;
using YourEasyRent.TelegramMenu.ButtonHandler;


namespace YourEasyRent.TelegramMenu
{
    public class TelegramSender : ITelegramSender 
    {
        private readonly ITelegramBotClient _botClient;
        private Dictionary<MenuStatus, IButtonHandler> _menus;
       // private readonly IProductRepository _productRepository;

        public TelegramSender(ITelegramBotClient botClient, IProductRepository productRepository)
        {
            _botClient = botClient;
            _menus = new Dictionary<MenuStatus, IButtonHandler>()
            {
                { MenuStatus.MainMenu, new MainMenuButtonHandler() },
                { MenuStatus.BrandMenu, new BrandButtonHandler(productRepository) },
                { MenuStatus.CategoryMenu, new CategoryButtonHandler() },
                { MenuStatus.MenuAfterReceivingRresult, new ReturnToMMButtonHandler() }
            };
        }
        public async Task SendMainMenu(string chatId)
        {
            var menu = await _menus[MenuStatus.MainMenu].SendMenuToTelegramHandle();
            await _botClient.SendTextMessageAsync(chatId, "Main menu. Choose one:", replyMarkup: menu);

        }
        public async Task SendBrandMenu(string chatId)
        {
            var menu = await _menus[MenuStatus.BrandMenu].SendMenuToTelegramHandle();
            await _botClient.SendTextMessageAsync(chatId, "Сhoose a brand:", replyMarkup: menu);
        }

        public async Task SendCategoryMenu(string chatId)
        {
            var menu = await _menus[MenuStatus.CategoryMenu].SendMenuToTelegramHandle();
            await _botClient.SendTextMessageAsync(chatId, "Сhoose a category:", replyMarkup: menu);
        }

        public async Task SendMenuAfterResult(string chatId)
        {
            var menu = await _menus[MenuStatus.MenuAfterReceivingRresult].SendMenuToTelegramHandle();
            await _botClient.SendTextMessageAsync(chatId, "What do you want to do next?", replyMarkup: menu);
        }
        public async Task SendResults(string chatId, List<Product> listWithResult)// ActionHandler Сюда пробросить, чтобы тут формировал ответ
        {
            var productsMessage = GetFilteredProductsMessage(listWithResult);
            await _botClient.SendTextMessageAsync(chatId, "Luk at ZEEEEZ!", replyMarkup: productsMessage);
        }

        private async Task<IEnumerable<string>> GetFilteredProductsMessage(List<Product> p) //  не знаю как для этого метода "распаковать" данные(бренд и категорию) из Листа,
                                                                                                         //  чтобы потом засунить их в аргемент метода GetProductsByBrandAndCategory
        {
            {
                var productStrings = products.Select(p =>
            $"*{p.Brand}*\n" +
            $"{p.Name}\n" +
            $"{p.Category}\n" +
            $"{p.Price}\n" +
            $"[.]({p.ImageUrl})\n" +
            $"[Ссылка на продукт]({p.Url})");
                return productStrings;
            }

        }

    }
    
}

using Amazon.Runtime.Internal.Transform;
using System.Security.Cryptography.X509Certificates;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
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
        private readonly IProductRepository _productRepository;

        public TelegramSender(ITelegramBotClient botClient, IProductRepository productRepository)
        {
            _botClient = botClient;
            _productRepository = productRepository;
            _menus = new Dictionary<MenuStatus, IButtonHandler>()
            {
                { MenuStatus.MainMenu, new MainMenuButtonHandler() },
                { MenuStatus.BrandMenu, new BrandButtonHandler(_productRepository) },
                { MenuStatus.CategoryMenu, new CategoryButtonHandler() },
                { MenuStatus.MenuAfterReceivingRresult, new ReturnToMMButtonHandler() },
                {MenuStatus.SubscribedToTheProduct, new SubscriptionButtonHandler() }
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

        public  async Task<IEnumerable<string>> SendResults(string chatId, List<string> listWithResult)
        {
            var resultsOfSearch = await GetFilteredProductsMessage(listWithResult);

            foreach (var result in resultsOfSearch)// этот результат который состоят только из строки Name и потом результат как то передавать агрументом в метод TransferDataToSubscriber
            {
               await _botClient.SendTextMessageAsync(chatId, $"Подписаться на {result} ", parseMode: ParseMode.Markdown); // только тут мы  вместо оправки результата в телегу я перекладываю полученные рещзультаты в новую бд
                                                         //идея заменить result на тест  $"Подписаться на {result} " - надо протестировать как будет выглядеть ответ                                                     
                                                                                                    
            }
            return resultsOfSearch;// но тут мы можем получить сразу несколько названий разных продуктов, поэтому пока думаю как с ними лучше работать !!
                                   // Лучше начать все-таки с одной позиции для отслеживания
        }

        public async Task<IEnumerable<string>> GetFilteredProductsMessage(List<string> listWithResult) 
        {
            var products = await _productRepository.GetProductsByBrandAndCategory(listWithResult);
            {
                var productStrings = products.Select(p =>
            $"*{p.Brand}*\n" +// мне нужно получить только Brand  and Name, чтобы передать эти данные в новую базу подписчиков,
                              // остельные поля можно не доставать. ИЛИ получаем только результат, чтобы потом не расклеивать строку
            $"{p.Name}\n" +
            $"{p.Category}\n" +
            $"{p.Price}\n" +
            $"[.]({p.ImageUrl})\n" +
            $"[Ссылка на продукт]({p.Url})");
                return productStrings;
            }
        }


        public async Task SendConfirmOfSubscriprion(string chatId)
        {
            var menu = await _menus[MenuStatus.SubscribedToTheProduct].SendMenuToTelegramHandle();
            await _botClient.SendTextMessageAsync(chatId, "The Product saved", replyMarkup: menu);
        }

        
    }
    
}

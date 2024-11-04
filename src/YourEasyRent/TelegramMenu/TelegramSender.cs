using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramBotAPI.Services;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Entities;
using YourEasyRent.Entities.ProductForSubscription;
using YourEasyRent.Services.Buttons;
using YourEasyRent.TelegramMenu.ButtonHandler;


namespace YourEasyRent.TelegramMenu
{
    public class TelegramSender : ITelegramSender 
    {
        private readonly ITelegramBotClient _botClient;
        private Dictionary<MenuStatus, IButtonHandler> _menus;
        private readonly IProductRepository _productRepository;
        private readonly IProductApiClient _client;
        public TelegramSender(ITelegramBotClient botClient, IProductRepository productRepository, IProductApiClient client)
        {
            _botClient = botClient;
            _productRepository = productRepository;
            _client = client;
            _menus = new Dictionary<MenuStatus, IButtonHandler>()
            {
                { MenuStatus.MainMenu, new MainMenuButtonHandler() },
                { MenuStatus.BrandMenu, new BrandButtonHandler(_client)}, // AK TODO этого репозитория тут уже не будет,  и я направляю запрос в сервис ProductApi,
                                                                                      // но не знаю как потом вернуть сообщение в нужный чат и как строить взаимодействие с TelegramCallbackHandler
                { MenuStatus.MenuAfterReceivingRresult, new ReturnToMMButtonHandler() },
                {MenuStatus.SubscribedToTheProduct, new SubscriptionButtonHandler() }
            };
        }
        public async Task SendMainMenu(string chatId)
        {
            var menu = await _menus[MenuStatus.MainMenu].SendMenuToTelegramHandle(chatId);
            await _botClient.SendTextMessageAsync(chatId, "Main menu. Choose one:", replyMarkup: menu);

        }
        public async Task SendBrandMenu(string chatId)
        {
            var menu = await _menus[MenuStatus.BrandMenu].SendMenuToTelegramHandle(chatId);
            await _botClient.SendTextMessageAsync(chatId, "Сhoose a brand:", replyMarkup: menu);
        }

        public async Task SendCategoryMenu(string chatId)
        {
            var menu = await _menus[MenuStatus.CategoryMenu].SendMenuToTelegramHandle(chatId);
            await _botClient.SendTextMessageAsync(chatId, "Сhoose a category:", replyMarkup: menu);
        }

        public async Task SendMenuAfterResult(string chatId)
        {
            var menu = await _menus[MenuStatus.MenuAfterReceivingRresult].SendMenuToTelegramHandle(chatId);
            await _botClient.SendTextMessageAsync(chatId, "What do you want to do next?", replyMarkup: menu);
        }

        public  async Task<IEnumerable<string>> SendResults(string chatId, List<string> listWithResult)
        {
            var resultsOfSearch = await _client.GetProductsResultForUser(listWithResult);

            foreach (var result in resultsOfSearch)
            {
               await _botClient.SendTextMessageAsync(chatId, result, parseMode: ParseMode.Markdown); // только тут мы  вместо оправки результата в телегу я перекладываю полученные результаты в новую бд                                                                                     
            }
            return resultsOfSearch;
        }

        //public async Task<IEnumerable<string>> GetFilteredProductsMessage(List<string> listWithResult) 
        //{
        //    var products = await _productRepository.GetProductsByBrandAndCategory(listWithResult);// AK TODO тут опять заменить 
        //    {
        //        var productStrings = products.Select(p =>
        //    $"*{p.Brand}*\n" +
        //    $"{p.Name}\n" +
        //    $"{p.Category}\n" +
        //    $"{p.Price}\n" +
        //    $"[.]({p.ImageUrl})\n" +
        //    $"[Ссылка на продукт]({p.Url})");
        //        return productStrings;
        //    }
        //}

        public async Task SendConfirmOfSubscriprion(string chatId)
        {
            var menu = await _menus[MenuStatus.SubscribedToTheProduct].SendMenuToTelegramHandle(chatId);
            await _botClient.SendTextMessageAsync(chatId, "The Product saved", replyMarkup: menu);
        }

        public async Task SendSubscriberProduct(string chatId, ProductForSubscription product)
        {
            var result = product.ToString(); 
            await _botClient.SendTextMessageAsync(chatId, $"WOW!The price of your product has become lower {result}", parseMode: ParseMode.Markdown);      
        }
    }
    
}

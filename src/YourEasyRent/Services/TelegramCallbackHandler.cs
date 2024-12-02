using YourEasyRent.Entities;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.UserState;
using YourEasyRent.TelegramMenu;
using YourEasyRent.Entities.ProductForSubscription;
using TelegramBotAPI.Services;

namespace YourEasyRent.Services
{
    public class TelegramCallbackHandler : ITelegramCallbackHandler
    {
        private readonly IUserStateRepository _userStateRepository;
        private readonly ITelegramSender _telegramSender;
        private readonly ILogger<TelegramCallbackHandler> _logger;
        private readonly IRabbitMessageProducer _rabbitMessageProducer;
        private readonly IProductApiClient _client;

        public TelegramCallbackHandler
            (
            ILogger<TelegramCallbackHandler> logger,
            IUserStateRepository userStateRepository,
            ITelegramSender telegramSender,
            IRabbitMessageProducer rabbitMessageProducer,
            IProductApiClient client
            )
        {
            _logger = logger;
            _userStateRepository = userStateRepository;
            _telegramSender = telegramSender;
            _rabbitMessageProducer = rabbitMessageProducer ?? throw new ArgumentNullException(nameof(rabbitMessageProducer));
            _client = client;
        }

        public async Task HandleUpdateAsync(TgButtonCallback tgButtonCallback)
        {

            var userId = tgButtonCallback.GetUserId();
            var chatId = tgButtonCallback.GetChatId();

            if (tgButtonCallback.IsStart)
            {
                var userSearchState = UserSearchState.CreateNewUserSearchState(userId);
                userSearchState.SetChatId(chatId);
                userSearchState.AddStatusToHistory(MenuStatus.MainMenu);
                await _userStateRepository.CreateAsync(userSearchState);
                await _telegramSender.SendMainMenu(chatId);
                return;
            };

            if (tgButtonCallback.IsValidMessage)
            {
                if (tgButtonCallback.IsValueMenuMessage)
                {
                    UserSearchState userSearchState = await _userStateRepository.GetForUser(userId);

                    if (tgButtonCallback.IsBrandMenu)
                    {
                        userSearchState.AddStatusToHistory(MenuStatus.BrandMenu);
                        await _userStateRepository.UpdateAsync(userSearchState);
                        var brandsList = await _client.GetBrandForMenu(chatId, 5); 
                        await _telegramSender.SendBrandMenu(chatId, brandsList);
                        return;
                    }

                    if (tgButtonCallback.IsCategoryMenu)
                    {                        
                        userSearchState.AddStatusToHistory(MenuStatus.CategoryMenu);
                        await _userStateRepository.UpdateAsync(userSearchState);
                        await _telegramSender.SendCategoryMenu(chatId);
                        return;
                    }

                }

                if (tgButtonCallback.IsValueProductButton)
                {
                    UserSearchState userSearchState = await _userStateRepository.GetForUser(userId);

                    if (tgButtonCallback.IsProductBrand)
                    {
                        userSearchState.AddStatusToHistory(MenuStatus.BrandChosen);
                        var brand = tgButtonCallback.GetProductButton();
                        userSearchState.SetBrand(brand);
                        await _userStateRepository.UpdateAsync(userSearchState);

                        if (userSearchState.IsFinished)
                        {
                            var tupleWithResult = await _userStateRepository.GetBrandAndCategoryForSearch(userId); 
                            var listWithResult = new List<string> { tupleWithResult.Brand, tupleWithResult.Category }.ToList();
                            var resultOfSearch = await _client.GetProductResultTuple(listWithResult);
                            userSearchState.SetProductNameAndPrice(resultOfSearch.Name, resultOfSearch.Price);
                            await _userStateRepository.UpdateAsync(userSearchState);
                            var resultOfSearchString = GetStringWithResult(resultOfSearch.Brand, resultOfSearch.Name, resultOfSearch.Price, resultOfSearch.Url);

                            await _telegramSender.SendOneResult(chatId, resultOfSearchString);
                            await _telegramSender.SendMenuAfterResult(chatId);  
                            return;
                        }
                        await _telegramSender.SendCategoryMenu(chatId); 
                        return;
                    };

                    if (tgButtonCallback.IsProductCategory)
                    {
                        userSearchState.AddStatusToHistory(MenuStatus.CategoryChosen);
                        var category = tgButtonCallback.GetProductButton();
                        userSearchState.SetCategory(category);
                        await _userStateRepository.UpdateAsync(userSearchState);

                        if (userSearchState.IsFinished)
                        {
                            var tupleWithResult = await _userStateRepository.GetBrandAndCategoryForSearch(userId); 
                            var listWithResult = new List<string> { tupleWithResult.Brand, tupleWithResult.Category }.ToList();
                            var resultOfSearch = await _client.GetProductResultTuple(listWithResult);
                            userSearchState.SetProductNameAndPrice(resultOfSearch.Name, resultOfSearch.Price);
                            await _userStateRepository.UpdateAsync(userSearchState);
                            var resultOfSearchString = GetStringWithResult(resultOfSearch.Brand, resultOfSearch.Name, resultOfSearch.Price, resultOfSearch.Url);

                            await _telegramSender.SendOneResult(chatId, resultOfSearchString); 
                            await _telegramSender.SendMenuAfterResult(chatId);
                            return;
                        }
                        var brandsList = await _client.GetBrandForMenu(chatId, 5);
                        await _telegramSender.SendBrandMenu(chatId, brandsList);
                        return;
                    };
                }
                if (tgButtonCallback.IsSubscribeToProduct) 
                {  
                    UserSearchState userSearchState = await _userStateRepository.GetForUser(userId);
                    userSearchState.AddStatusToHistory(MenuStatus.SubscribedToTheProduct);
                    await _userStateRepository.UpdateAsync(userSearchState);
                    var subscriber = ProductForSubscription.CreateProductForSubscription(userSearchState);
                    _rabbitMessageProducer.SendMessagAboutSubscriber(subscriber);
                    await _telegramSender.SendConfirmOfSubscriprion(chatId);
                    return;
                }
            }
        }
        public string GetStringWithResult(string? Brand, string? Name, decimal? Price, string? Url)
        {
            string brandPart = Brand;
            string namePart = Name;
            string pricePart = Price.ToString();
            string urlPart = Url;

             string productString =
                $"*{brandPart}*\n" +
                $"{namePart}\n" +
                $"{pricePart}\n" +               
                $"[Ссылка на продукт]({urlPart})";
            return productString;
        }
    }
}
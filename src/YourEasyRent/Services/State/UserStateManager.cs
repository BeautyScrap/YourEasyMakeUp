using YourEasyRent.Services.Buttons;

namespace YourEasyRent.Services.State
{
    public class UserStateManager : IUserStateManager
    {
       
        // class tgSender // ответственность только пульнуть нужное меню в телегу, а не собирать данные, в ракках телеграмХендлера из репозитория он получает нужные данные

        // SendCategoryMenu()// откуда мы должны брать эти листы categoryList итд?
        // SendBrandMenu(brandList)
        // SendResults(results)


        // class TgSender(botClient): ITgSender
        //   _menues = new Dictionary<string, IButtonHandler>()
        //        { "CategoryMenu", new CategoryButtonHandler(_botClient)  },
        //...
        // SendCategoryMenu(){
        //  var menu = _menues["CategoryMenu"].GetMatckup() => new InlineKeyboardMarkup(.....);
        //  return await _botClient.SendTextMessageAsync(chatId, "Сhoose a category:", replyMarkup: menu);
        // 
        //}
        public bool IsCategory { get; private set; }// await tgSender.SendMenu(nextMenu);en { get; private set; }
        public bool IsBackOnOneStep { get; private set; }
        public bool IsReturnToMainMenu { get; private set; }

        private List<string> menuStatuses = new List<string>();
        public void Start()
        {
            menuStatuses.Add("Started");
        }
        public void SetBrand(MenuStatus status)
        {
            IsBrandChosen = true;
            menuStatuses.Add("BrandChosen");
        }
        public void SetCategory(MenuStatus status)
        {
            IsCategoryChosen = true;
            menuStatuses.Add("CategoryChosen");
        }
        public void MethodBackOnOneStep(string status)
        {
            IsBackOnOneStep = true;
            menuStatuses.Last();
            menuStatuses.Remove(status);
        }
        public void ReturnToMainMenu()
        {
            IsReturnToMainMenu = true;
            menuStatuses.Clear();
            menuStatuses.Add("Started");
        }
        public void AddStatusToList(string status)                                                 
        {
            if (!menuStatuses.Contains(status))
            {
                menuStatuses.Add($"{status}");
            }
            if (menuStatuses.Contains("ReturnToMainMenu"))
            {
                menuStatuses.Clear();
                menuStatuses.Add("Started");
            }
        }
        public string GetNextStep(string status) // или тут object. Еще возмодно стоит сделать метод ассинхронным
        {
            if (menuStatuses.Contains("BrandChosen") && menuStatuses.Contains("CategoryChosen"))
            {
                return "ReadyToResult";
            }
            if (menuStatuses.Contains("BrandChosen"))
            {
                if (!menuStatuses.Contains("CategoryChosen"))
                {
                    return "CategoryMenu";
                }
                return "ReadyToResult";
            }
            if (menuStatuses.Contains("CategoryChosen"))
            {
                if (!menuStatuses.Contains("BrandChosen"))
                {
                    return "CategoryMenu";
                }
                return "ReadyToResult";
            }
            return "ReadyToResult";
        }
        public string GetPreviousStep()
        {
            return menuStatuses.Last();
        }
    }
}

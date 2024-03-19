namespace YourEasyRent.Services.State
{
    public class UserStateManager: IUserStateManager
    {

        public bool IsCategoryChosen { get; private set; }
        public bool IsBrandChosen { get; set; }
        public bool IsBackOnOneStep {  get; set; }  
        public bool IsReturnToMainMenu { get; set; }    

        private List<string> menuStatuses = new List<string>();

        public void Started()
        { 
            menuStatuses.Add("Started");
        }
        public void BrandChosen(MenuStatus status)
        {
            IsBrandChosen = true;
            menuStatuses.Add("BrandChosen");
        }

        public void CategoryChosen(MenuStatus status) 
        {
            IsCategoryChosen = true;
            menuStatuses.Add("CategoryChosen");
        }
         public void BackOnOneStep()
        {
            IsBackOnOneStep = true;
            menuStatuses.Last();
        }
         public void ReturnToMainMenu()
        {
            IsReturnToMainMenu = true;
            menuStatuses.Clear();
            menuStatuses.Add("MainMenu");
        }
        
        public void AddStatusToList(string status)//  в листе должны быть уникальные значения,
                                                  //  а после того как мы нажимаем кнопку returnTOMenu\\ или лучше StartAgain лист чистится и ставиться на позицию Started
        {
            if (!menuStatuses.Contains(status))
            {
                menuStatuses.Add($"{status}");
            }
            if (menuStatuses.Contains("ReturnToMainMenu") )
            {
                menuStatuses.Clear();
                menuStatuses.Add("Started");
            }
            
        }

        public string CheckStatusInList(string status) // или тут object. Еще возмодно стоит сделать метод ассинхронным
        {
            if(menuStatuses.Contains("BrandChosen") && menuStatuses.Contains("CategoryChosen"))
            {
                return "ReadyToResul";
                // наверно, лучше вернуть статут ReadyToResul, который потом даст команду к запуску фианльно запроса к бд
            }
            if (menuStatuses.Contains("BrandChosen"))
            {
                if (!menuStatuses.Contains("CategoryChosen"))
                {
                    return "CategoryMenu";
                }
                return "ReadyToResul";// написать проверку для цены, !menuStatuses.Contains("PriceChosen")
            }
            if (menuStatuses.Contains("CategoryChosen"))
            {
                if (!menuStatuses.Contains("BrandChosen"))
                {
                    return "CategoryMenu";
                }
                return "ReadyToResul";// написать проверку для цены, !menuStatuses.Contains("PriceChosen")
            }

            return "ReadyToResul";
        }
    }
}

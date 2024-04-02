namespace YourEasyRent.Services.State
{
    public class UserStateManager : IUserStateManager
    {
        public bool IsCategoryChosen { get; private set; }
        public bool IsBrandChosen { get; private set; }
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
        public void GoBackToOneStep(string status)
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
        public string CheckStatusInList(string status) // или тут object. Еще возмодно стоит сделать метод ассинхронным
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
        public string MethodBackOnOneStep()
        {
            return menuStatuses.Last();
        }
    }
}

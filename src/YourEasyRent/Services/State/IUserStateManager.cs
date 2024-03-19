namespace YourEasyRent.Services.State
{
    public interface IUserStateManager
    {
        void Started();

        void BrandChosen(MenuStatus status);

        void CategoryChosen(MenuStatus status);

        void BackOnOneStep();

        void ReturnToMainMenu();


        void AddStatusToList(string status);
        public string CheckStatusInList(string status);
    }
}

namespace YourEasyRent.Services.State
{
    public interface IUserStateManager
    {
        void Started();

        void BrandChosen(MenuStatus status);

        void CategoryChosen(MenuStatus status);

        void BackOnOneStep(string status);

        void ReturnToMainMenu();


        void AddStatusToList(string status);
        public string CheckStatusInList(string status);
        public string MethodBackOnOneStep();
    }
}

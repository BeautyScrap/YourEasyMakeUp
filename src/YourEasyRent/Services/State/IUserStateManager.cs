namespace YourEasyRent.Services.State
{
    public interface IUserStateManager
    {
        void Start();

        void SetBrand(MenuStatus status);

        void SetCategory(MenuStatus status);

        void GoBackToOneStep(string status);

        void ReturnToMainMenu();


        void AddStatusToList(string status);
        public string CheckStatusInList(string status);
        public string MethodBackOnOneStep();
    }
}

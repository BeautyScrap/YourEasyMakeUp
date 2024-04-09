namespace YourEasyRent.Services.State
{
    public interface IUserStateManager
    {
        void Start();

        void SetBrand(MenuStatus status);

        void SetCategory(MenuStatus status);

        void MethodBackOnOneStep(string status);

        void ReturnToMainMenu();


        void AddStatusToList(string status);
        public string GetNextStep(string status);
        public string GetPreviousStep(); //  use an enum stead of a string
    }
}

using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace YourEasyRent.Services.State
{
    public interface IUserSearchState
    {
        //void StartSearch();
        void SetBrand(string brand);
        void SetCategory (string category);
        void BackOnPreviousStep();
        void GetNextMenu();
        void IsFiniShed();
        UserSearchStateDTO TOMongoRepresintation();


    }
}

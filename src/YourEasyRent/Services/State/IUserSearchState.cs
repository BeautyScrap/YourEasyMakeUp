using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace YourEasyRent.Services.State
{
    public interface IUserSearchState
    {
        //void StartSearch();
        void SetChatId(long chatId);    
        void SetBrand(string brand);
        void SetCategory (string category);
        void BackOnPreviousStep(MenuStatus status); //  может в агрументы еще chatid / userid занести, чтобы точно
                                                    //  знать у какого пользователя сменить статус на предыдущий
        void SetStatus(MenuStatus status);
        void GetNextMenu();
        void IsFiniShed();
        UserSearchStateDTO TOMongoRepresintation();


    }
}

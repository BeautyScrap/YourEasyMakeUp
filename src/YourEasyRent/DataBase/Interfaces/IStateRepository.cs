using Telegram.Bot.Types;
using YourEasyRent.Services.State;
using MongoDB.Driver;


namespace YourEasyRent.DataBase.Interfaces
{
    public interface IStateRepository
    {

        Task<UserSearchState> GetForUser(long userId);
        Task Save(UserSearchState userSearchState);

    }
}

using Telegram.Bot.Types;
using YourEasyRent.Services.State;
using MongoDB.Driver;


namespace YourEasyRent.DataBase.Interfaces
{
    public interface IUserStateManagerRepository
    {
        Task<UserSearchState> GetForUser(long userId);
        Task CreateAsync(UserSearchState userSearchState);

        Task UpdateAsync(UserSearchState userSearchState);
        Task<MenuStatus> GetCurrentStateForUser(long userId);
    }
}

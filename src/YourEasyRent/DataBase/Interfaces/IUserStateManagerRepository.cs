using Telegram.Bot.Types;
using MongoDB.Driver;
using YourEasyRent.Entities;
using YourEasyRent.UserState;

namespace YourEasyRent.DataBase.Interfaces
{
    public interface IUserStateManagerRepository
    {
        Task<UserSearchState> GetForUser(string userId);
        Task CreateAsync(UserSearchState userSearchState);

        Task UpdateAsync(UserSearchState userSearchState);
        Task<MenuStatus> GetCurrentStateForUser(string userId);
    }
}

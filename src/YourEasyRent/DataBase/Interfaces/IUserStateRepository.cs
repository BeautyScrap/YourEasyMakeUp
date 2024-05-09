using Telegram.Bot.Types;
using MongoDB.Driver;
using YourEasyRent.Entities;
using YourEasyRent.UserState;

namespace YourEasyRent.DataBase.Interfaces
{
    public interface IUserStateRepository
    {
        Task<UserSearchState> GetForUser(string userId);
        Task CreateAsync(UserSearchState userSearchState,string userId, MenuStatus status);

        Task<bool> UpdateAsync(UserSearchState userSearchState, string userId, MenuStatus status);
        Task<MenuStatus> GetCurrentStateForUser(string userId);
    }
}

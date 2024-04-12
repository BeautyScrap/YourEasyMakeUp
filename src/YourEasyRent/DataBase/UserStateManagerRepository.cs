using MongoDB.Driver;
using Telegram.Bot.Types;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Services.State;

namespace YourEasyRent.DataBase
{
    public class UserStateManagerRepository : IUserStateManagerRepository
    {
        private readonly IMongoCollection<UserSearchStateDTO> _collectionOfUserSearchState;
        public UserStateManagerRepository(DataBaseConfig dateBaseConfig, IMongoClient mongoClient)
        {
            var dataBase = mongoClient.GetDatabase(dateBaseConfig.DataBaseName);
            _collectionOfUserSearchState = dataBase.GetCollection<UserSearchStateDTO>(dateBaseConfig.CollectionName);
        }

        public async Task<UserSearchState> GetForUser(long userId)
        {
            var filter = Builders<UserSearchStateDTO>.Filter.Eq(u => u.UserId, userId);

            var dto = await _collectionOfUserSearchState.Find(filter).FirstOrDefaultAsync();

            var state = new UserSearchState(dto);

            return state;
        }

        public async Task CreateAsync(UserSearchState userSearchState)
        {
            var dto = userSearchState.TOMongoRepresintation();
            await _collectionOfUserSearchState.InsertOneAsync(dto);
        }

        public async Task UpdateAsync(UserSearchState userSearchState)
        {
            var filter = Builders<UserSearchStateDTO>.Filter.Eq(u => u.UserId, userSearchState.UserId);
            var dto = userSearchState.TOMongoRepresintation();
            await _collectionOfUserSearchState.ReplaceOneAsync(filter, dto);
        }
    }
}

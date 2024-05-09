using MongoDB.Driver;
using Telegram.Bot.Types;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Entities;
using YourEasyRent.UserState;

namespace YourEasyRent.DataBase
{
    public class UserStateRepository : IUserStateRepository
    {
        private readonly IMongoCollection<UserSearchStateDTO> _collectionOfUserSearchState;
        public UserStateRepository(DataBaseConfig dateBaseConfig, IMongoClient mongoClient)
        {
            var dataBase = mongoClient.GetDatabase(dateBaseConfig.DataBaseName);
            _collectionOfUserSearchState = dataBase.GetCollection<UserSearchStateDTO>("UsersDataCollection");
        }

        public async Task<UserSearchState> GetForUser(string userId)
        {
            
            var filter = Builders<UserSearchStateDTO>.Filter.Eq(u => u.UserId, userId);

            var dto = await _collectionOfUserSearchState.Find(filter).FirstOrDefaultAsync();

            var state = new UserSearchState(dto);

            return state;
        }

        public async Task CreateAsync(UserSearchState userSearchState, string userId, MenuStatus status)
        {
            var dto = userSearchState.ToMongoRepresintation(userId, status);
            await _collectionOfUserSearchState.InsertOneAsync(dto);
        }

        public async Task<bool> UpdateAsync(UserSearchState userSearchState, string userId, MenuStatus status)
        {
            var filter = Builders<UserSearchStateDTO>.Filter.Eq(u => u.UserId, userSearchState.UserId);
            var update = Builders<UserSearchStateDTO>.Update
                .Set(u => u.UserId, userSearchState.UserId)
                .Set(u => u.Brand, userSearchState.Brand)
                .Set(u => u.Category, userSearchState.Category)
                .Set(u => u.Status, status)
                .Set(u => u.HistoryOfMenuStatuses, userSearchState.HistoryOfMenuStatuses);

            //var dto = userSearchState.ToMongoRepresintation(userId, status);
            var updateResult = await _collectionOfUserSearchState.UpdateOneAsync(filter, update);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<MenuStatus> GetCurrentStateForUser(string userId)
        {
            var filter = Builders<UserSearchStateDTO>.Filter.Eq(u =>u.UserId, userId);
            var dto = await _collectionOfUserSearchState.Find(filter).FirstOrDefaultAsync();

            var state = new UserSearchState(dto);

            return state.CurrentMenuStatus;// не уверена, надо при запуске посмотреть что возвращает метод, точно ли корректный статус

        }
    }
}

using MongoDB.Driver;
using System.Collections.Generic;
using Telegram.Bot.Types;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Entities;
using YourEasyRent.Services.Buttons;
using YourEasyRent.TelegramMenu.ButtonHandler;
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

        public async Task CreateAsync(UserSearchState userSearchState)
        {
            var dto = userSearchState.ToDto();
            await _collectionOfUserSearchState.InsertOneAsync(dto);
        }

        public async Task<bool> UpdateAsync(UserSearchState userSearchState)
        {
            var filter = Builders<UserSearchStateDTO>.Filter.Eq(u => u.UserId, userSearchState.UserId);
            var update = Builders<UserSearchStateDTO>.Update
                .Set(u => u.UserId, userSearchState.UserId)
                .Set(u => u.Brand, userSearchState.Brand)
                .Set(u => u.Category, userSearchState.Category)
                .Set(u => u.Status, userSearchState.CurrentMenuStatus)
                .Set(u => u.HistoryOfMenuStatuses, userSearchState.HistoryOfMenuStatuses);

            var updateResult = await _collectionOfUserSearchState.UpdateOneAsync(filter, update);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<MenuStatus> GetCurrentStateForUser(string userId)
        {
            var filter = Builders<UserSearchStateDTO>.Filter.Eq(u =>u.UserId, userId);
            var dto = await _collectionOfUserSearchState.Find(filter).FirstOrDefaultAsync();

            var state = new UserSearchState(dto);

            return state.CurrentMenuStatus;

        }

        public async Task<bool> CheckFieldsFilledForUser(string userId) // проверяет заполнены ли нужные поля у юзера
        {
            var filter = Builders<UserSearchStateDTO>.Filter.Eq(u => u.UserId, userId);
            var dto = await _collectionOfUserSearchState.Find(filter).FirstOrDefaultAsync();

            if (dto == null)
            {
                return false;
            }
            return true;

        }

        public async Task<(string Brand, string Category)> GetFilteredProductsForSearch(string userId)// этот метод остается как есть тк нам нужна категория и бренд,
                                                                                                      // чтобы найти продукт в другой базе
        {
            var filter = Builders<UserSearchStateDTO>.Filter.Eq(u => u.UserId, userId);
            var projection = Builders<UserSearchStateDTO>.Projection.Include(u =>u.Brand).Include(u => u.Category);
            var brandAndCategoryResult = await _collectionOfUserSearchState.Find(filter).Project(u =>new { u.Brand, u.Category} ).FirstOrDefaultAsync();

            return (brandAndCategoryResult.Brand, brandAndCategoryResult.Category);

        }
    }
}

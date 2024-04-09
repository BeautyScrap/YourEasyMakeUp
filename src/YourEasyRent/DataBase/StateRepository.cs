using MongoDB.Driver;
using Telegram.Bot.Types;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Services.State;

namespace YourEasyRent.DataBase
{
    public class StateRepository : IStateRepository
    {
        private readonly IMongoCollection<UserSearchState> _collectionOfUserSearchState;
        public StateRepository(DataBaseConfig dateBaseConfig,IMongoClient mongoClient )
        {
            var dataBase = mongoClient.GetDatabase(dateBaseConfig.DataBaseName);
            _collectionOfUserSearchState = dataBase.GetCollection<UserSearchState>(dateBaseConfig.CollectionName);
        }

        public async Task<UserSearchState> GetForUser(long userId) //логика получения данных о пользователе из монги
        {
            return await _collectionOfUserSearchState.Find<UserSearchState>(_ => _.UserId == userId).FirstOrDefault();   //из за DTO  не понимаю к какой коллекции обратиться!
        }

        public Task Save(UserSearchState userSearchState)  // //логика сохранения данных о пользователе в монги
        {
            throw new NotImplementedException();
        }
    }
}

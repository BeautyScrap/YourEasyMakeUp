using MongoDB.Driver;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Entities;

namespace YourEasyRent.DataBase
{
    public class SubscribersRepository : ISubscribersRepository
    {

        private readonly IMongoCollection<SubscribersDto> _subscribersCollection;
        public SubscribersRepository(DataBaseConfig dataBaseConfig, IMongoClient mongoClient)
        {
            var dataBase = mongoClient.GetDatabase(dataBaseConfig.DataBaseName);
            _subscribersCollection = dataBase.GetCollection<SubscribersDto>("CollectionOfSubscribers");
        }
        public async Task CreateSubscriberAsync(Subscriber subscriber)
        {
            var dto = subscriber.ToMongoRepresentationSubscriber();
            await _subscribersCollection.InsertOneAsync(dto);
        }

        public async Task<Subscriber> GetSubscriberAsync(string UserId)
        {        
            var filter = Builders<SubscribersDto>.Filter.Eq(subscriber => subscriber.UserId, UserId);
            var dto = await _subscribersCollection.Find(filter).FirstOrDefaultAsync();
            var subscriber = new Subscriber(dto);
            return subscriber;
        }
    }
}

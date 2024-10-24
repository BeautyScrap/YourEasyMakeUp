//using MongoDB.Driver;
//using YourEasyRent.DataBase.Interfaces;
//using YourEasyRent.Entities.ProductForSubscription;

//namespace YourEasyRent.DataBase
//{
//    public class SubscribersRepository : ISubscribersRepository
//    {

//        private readonly IMongoCollection<ProductForSubscriptionDto> _subscribersCollection;
//        public SubscribersRepository(DataBaseConfig dataBaseConfig, IMongoClient mongoClient)
//        {
//            var dataBase = mongoClient.GetDatabase(dataBaseConfig.DataBaseName);
//            _subscribersCollection = dataBase.GetCollection<ProductForSubscriptionDto>("CollectionOfSubscribers");
//        }
//        public async Task CreateSubscriberAsync(ProductForSubscription subscriber)
//        {
//            var dto = subscriber.ToDto();
//            await _subscribersCollection.InsertOneAsync(dto);
//        }

//        public async Task<ProductForSubscription> GetSubscriberAsync(string UserId)
//        {        
//            var filter = Builders<ProductForSubscriptionDto>.Filter.Eq(subscriber => subscriber.UserId, UserId);
//            var dto = await _subscribersCollection.Find(filter).FirstOrDefaultAsync();
//            var subscriber = new ProductForSubscription();
//            return subscriber;
//        }
//    }
//}

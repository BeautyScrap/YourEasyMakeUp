using SubscriberAPI.Domain;
using SubscriberAPI.Infrastructure;

namespace SubscriberAPI.Application
{
    public class SubscriberService : ISubscrieberService
    {
        private readonly ISubscribersRepository _subscribersRepository;

        public SubscriberService(ISubscribersRepository subscribersRepository)
        {
            _subscribersRepository = subscribersRepository ?? throw new ArgumentNullException(nameof(subscribersRepository));
        }
        public async Task Create(Subscription subscription)
        {
            var subscriptionDto = subscription.ToDBRepresentation();
            await _subscribersRepository.CreateAsync(subscriptionDto);//  получаем подписчика и тут переделываем этот объект в дто
        }

        public async Task<IEnumerable<SubscriptionDto>> GetAllAsync()
        {
        
            var subscribers = (await _subscribersRepository.GetAllSubscribersAsync()).ToList();
            var subDtos = new List<SubscriptionDto>();
            subscribers.ForEach(subscriber =>
            {
                var subDto = new SubscriptionDto
                {
                    UserId = subscriber.UserId,
                    ChatId = subscriber.ChatId,
                    Brand = subscriber.Brand,
                    Name = subscriber.Name,
                    Price = subscriber.Price,
                    Url = subscriber.Url
                };
                subDtos.Add(subDto);
            });
            return subDtos;  
        }

        public async Task<SubscriptionDto> GetById(string userId)
        {
            Subscription subscriber = await _subscribersRepository.GetSubscriberAsync(userId);// при получении данных все поля кроме
            var subDto = subscriber.ToDBRepresentation();                                                                                // price и Url становятся null, нужел ли automapping?
            return subDto;
        }

        public async Task<bool> Update(string userId, Subscription newSubscriber)
        {
            //Subscriber subscriber = await _subscribersRepository.GetSubscriberAsync(userId);
            //if (subscriber == null)
            //{
            //    return false;
            //}
            var updateResult = await _subscribersRepository.UpdateAsync(newSubscriber);
            return updateResult > 0;
        }
        public async Task<bool> Delete(string userId)
        {
            Subscription subscriber = await _subscribersRepository.GetSubscriberAsync(userId);
            if (subscriber == null)
            {
                return false;
            }
            var updateResult = await _subscribersRepository.DeleteAsync(userId);
            return updateResult > 0;
        }

        public async Task<List<Subscription>> GetFieldsForSearchById()
        {
            var subscribers = (await _subscribersRepository.GetFieldsForSearchAsync()).ToList();
            var listSubscriprions= new List<Subscription>();

            foreach (var subscriber in subscribers)
            {
                listSubscriprions.Add(subscriber);
            }
            return listSubscriprions;

            //subscribers.ForEach(subscriber =>
            //{
            //    var subDto = new Subs
            //    {
            //        UserId = subscriber.UserId,
            //        Brand = subscriber.Brand,
            //        Name = subscriber.Name,
            //        Price = subscriber.Price,
            //    };
            //    subDtos.Add(subDto);
            //});
            //return subDtos;
        }
    }
}

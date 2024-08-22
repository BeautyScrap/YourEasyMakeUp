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
        public async Task Create(Subscriber newSubscriber)
        {
            await _subscribersRepository.CreateAsync(newSubscriber);
        }

        public async Task<IEnumerable<SubscriberDto>> GetAllAsync()
        {
        
            var subscribers = (await _subscribersRepository.GetAllSubscribersAsync()).ToList();
            var subDtos = new List<SubscriberDto>();
            subscribers.ForEach(subscriber =>
            {
                var subDto = new SubscriberDto
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

        public async Task<SubscriberDto> GetById(string userId)
        {
            Subscriber subscriber = await _subscribersRepository.GetSubscriberAsync(userId);// при получении данных все поля кроме
            var subDto = subscriber.CreateSubscriberDtoObject();                                                                                // price и Url становятся null, нужел ли automapping?
            return subDto;
        }

        public async Task<bool> Update(string userId, Subscriber newSubscriber)
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
            Subscriber subscriber = await _subscribersRepository.GetSubscriberAsync(userId);
            if (subscriber == null)
            {
                return false;
            }
            var updateResult = await _subscribersRepository.DeleteAsync(userId);
            return updateResult > 0;
        }

        public async Task<List<SubscriberDto>> GetFieldsForSearchById()
        {
            var subscribers = (await _subscribersRepository.GetFieldsForSearchAsync()).ToList();
            var subDtos = new List<SubscriberDto>();
            subscribers.ForEach(subscriber =>
            {
                var subDto = new SubscriberDto
                {
                    UserId = subscriber.UserId,
                    Brand = subscriber.Brand,
                    Name = subscriber.Name,
                    Price = subscriber.Price,
                };
                subDtos.Add(subDto);
            });
            return subDtos;
        }
    }
}

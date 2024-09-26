using Microsoft.AspNetCore.Mvc;
using SubscriberAPI.Domain;
using SubscriberAPI.Infrastructure;

namespace SubscriberAPI.Application
{
    public class SubscriberService : ISubscrieberService // по идеи тут я возвращаю именно Subscription, а в репозитории именно SubscriptionDto
    {
        private readonly ISubscribersRepository _subscribersRepository;

        public SubscriberService(ISubscribersRepository subscribersRepository)
        {
            _subscribersRepository = subscribersRepository ?? throw new ArgumentNullException(nameof(subscribersRepository));
        }
        public async Task Create(Subscription subscription)
        {
            var subscriptionDto = subscription.ToDto();
            await _subscribersRepository.CreateAsync(subscriptionDto);//  получаем подписчика и тут переделываем этот объект в дто
        }

        public async Task<IEnumerable<Subscription>> GetAllAsync()
        {
            var subscribersDtos = (await _subscribersRepository.GetAllSubscribersAsync()).ToList();
            var subscriptions = subscribersDtos.Select(dto => Subscription.CreateNewSubscription(
                dto.UserId,
                dto.ChatId,
                dto.Brand,
                dto.Name,
                dto.Price,
                dto.Url
                )).ToList();
            return subscriptions;
        }

        public async Task<Result<Subscription>> GetById(string userId)
        {
            var subscriberDto = await _subscribersRepository.GetSubscriberAsync(userId);
            if( subscriberDto is null ) 
            {
                var message = $"The subscription with userId {userId} is not found";
                return Result<Subscription>.Failure(Error.NotFound(message));            
            }
            var subscription = Subscription.CreateNewSubscription(
                subscriberDto.UserId,
                subscriberDto.ChatId,
                subscriberDto.Brand,
                subscriberDto.Name,
                subscriberDto.Price,
                subscriberDto.Url
                );
            return Result<Subscription>.Success(subscription);
        }

        public async Task<bool> Update(string userId, Subscription subscription)
        {
            var subscriptionDto = subscription.ToDto();
            var subscriberByUserId = await _subscribersRepository.GetSubscriberAsync(userId);
            if (subscriberByUserId == null)
            {
                return false;
            }
            var updateResult = await _subscribersRepository.UpdateAsync(subscriptionDto);
            return updateResult > 0;
        }
        public async Task<bool> Delete(string userId)
        {
            var updateResult = await _subscribersRepository.DeleteAsync(userId);
            return updateResult > 0;
        }

        public async Task<List<Subscription>> GetFieldsForSearchById()
        {
            var subscribersDtos = (await _subscribersRepository.GetFieldsForSearchAsync()).ToList();
            var listOfSubscription = subscribersDtos.Select(dto => Subscription.CreateNewSubscription(
                dto.UserId,
                dto.ChatId,
                dto.Brand,
                dto.Name,
                dto.Price,
                dto.Url
                )).ToList();
            return listOfSubscription;

            //var listSubscriprions= new List<Subscription>();

            //foreach (var subscriber in subscribers)
            //{
            //    listSubscriprions.Add(subscriber);
            //}
            //return listSubscriprions;

        }
    }
}

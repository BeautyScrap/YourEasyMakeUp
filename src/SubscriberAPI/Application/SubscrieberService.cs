using Microsoft.AspNetCore.Mvc;
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

            await _subscribersRepository.CreateAsync(subscription);
        }

        public async Task<IEnumerable<Subscription>> GetAllAsync()
        {
            var subscribersDtos = (await _subscribersRepository.GetAllSubscribersAsync()).ToList();
            var subscriptions = subscribersDtos.Select(dto => Subscription.CreateNewSubscription(
                dto.UserId,
                dto.ChatId,
                dto.Brand,
                dto.Name,
                dto.Price
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
                subscriberDto.Price
                );
            return Result<Subscription>.Success(subscription);
        }

        public async Task<bool> Update(string userId, Subscription subscription)
        {
            var subscriberByUserId = await _subscribersRepository.GetSubscriberAsync(userId);
            if (subscriberByUserId == null)
            {
                return false;
            }
            var updateResult = await _subscribersRepository.UpdateAsync(subscription);
            return updateResult > 0;

        }
        public async Task<bool> Delete(string userId)
        {
            var updateResult = await _subscribersRepository.DeleteAsync(userId);
            return updateResult > 0;
        }

        public async Task<List<Subscription>> GetFieldsForSearchById()
        {
            var subscribers = (await _subscribersRepository.GetFieldsForSearchAsync()).ToList();    

            return subscribers;
        }

        public async Task<bool> UpdateStatusForFoundProduct(string userId, string name)
        {
           return await _subscribersRepository.UpdateStatusFoundProduct(userId, name) > 0;
        }
    }
}

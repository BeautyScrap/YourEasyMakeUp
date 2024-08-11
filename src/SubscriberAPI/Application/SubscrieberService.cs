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
            await _subscribersRepository.Create(newSubscriber);
        }

        public async Task<IEnumerable<Subscriber>> GetAllAsync()
        {
            List<Subscriber> subscribers = new List<Subscriber>();
            subscribers = (await _subscribersRepository.GetSubscribersAsync()).ToList();
            return subscribers.ToList();
        }

        public Task<Subscriber> GetById(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(string userId, Subscriber newSubscriber)
        {
            throw new NotImplementedException();
        }
        public Task<bool> Delete(Subscriber newSubscriber)
        {
            throw new NotImplementedException();
        }
    }
}

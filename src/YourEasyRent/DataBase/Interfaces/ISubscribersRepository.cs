using YourEasyRent.Entities;

namespace YourEasyRent.DataBase.Interfaces
{
    public interface ISubscribersRepository
    {
        Task<Subscriber> GetSubscriberAsync(string UserId);
        Task CreateSubscriberAsync(Subscriber subscribers);

    }
}

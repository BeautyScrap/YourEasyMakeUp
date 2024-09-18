using SubscriberAPI.Domain;

namespace SubscriberAPI.Application
{
    public interface ISubscrieberService
    {
        Task<IEnumerable<Subscription>> GetAllAsync(); // по идеи тут мы должны вернуть объект Subscription,
                                                          // а внутри самого метода преобразуем из Dto в Subscription                                                         // и возвращаем объект в контроллер и там преобразуем его в response
        Task<Subscription> GetById(string userId);
        Task Create(Subscription subscription);
        Task<bool> Update(string userId, Subscription subscription);
        Task<bool> Delete(string userId);
        Task<List<Subscription>> GetFieldsForSearchById();

    }
}

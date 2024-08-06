using SubscriberAPI.Domain;
using Dapper;
using System.Data;
using Npgsql;

namespace SubscriberAPI.Infrastructure
{
    public class SubscribersRepository : ISubscribersRepository
    {
        private readonly string _connectionString; 

        public SubscribersRepository(string connectonString)
        {
            _connectionString = connectonString;
        }

        public async Task Create(Subscriber newSubscriber)// метод ничего не возвращает,а мог бы быть bool, чтобы сделать проверку,
                                                          // что пользователь создался в бд и все ок и прокинуть этот ответ в контроллер.
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = 
                    @"INSERT INTO Subscribers (UserId, ChatId, Category, Brand, Name, Price, Url)
                    VALUES (@UserId, @ChatId, @Category, @Brand, @Name, @Price, @Url)";
               await connection.ExecuteAsync(query, newSubscriber);
            }
        }


        public Task<Subscriber> GetSubscriberAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Subscriber>> GetSubscribersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Subscriber newSubscriber)
        {
            throw new NotImplementedException();
        }
    }
}

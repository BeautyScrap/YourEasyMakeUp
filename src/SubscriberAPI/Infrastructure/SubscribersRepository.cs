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

        public async Task CreateAsync(SubscriptionDto subscriptionDto)// AK TODO проверить как будет вести себя поле url  при null                                                              
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query =
                    @"INSERT INTO Subscribers (user_id, chat_id, brand_product, name_product, price, url)
                    VALUES (@UserId, @ChatId, @Brand, @Name, @Price, @Url)";
                await connection.ExecuteAsync(query, subscriptionDto);
            }
        }

        public async Task<IEnumerable<SubscriptionDto>> GetAllSubscribersAsync() 
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"
            SELECT 
                ""user_id"" AS UserId,
                ""chat_id"" AS ChatId,
                ""brand_product"" AS Brand,
                ""name_product"" AS Name,
                ""price"" AS Price,
                ""url"" AS Url
            FROM Subscribers";
                return await connection.QueryAsync<SubscriptionDto>(query); 
            }
        }
        public async Task<SubscriptionDto> GetSubscriberAsync(string userId) 
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query =
                    @"SELECT
                ""user_id"" AS UserId,
                ""chat_id"" AS ChatId,
                ""brand_product"" AS Brand,
                ""name_product"" AS Name,
                ""price"" AS Price,
                ""url"" AS Url
                FROM public.subscribers
                WHERE user_id = @userId";
                return await connection.QuerySingleOrDefaultAsync<SubscriptionDto>(query, new { userId });
            }
        }
        public async Task<int> UpdateAsync(SubscriptionDto subscriptionDto)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query =
                    @"UPDATE public.Subscribers 
                      SET user_id = @UserId, 
                            chat_id = @ChatId, 
                            brand_product = @Brand, 
                            name_product = @Name, 
                            price = @Price, 
                            url = @Url 
                      WHERE user_id = @userId";
                return await connection.ExecuteAsync(query, subscriptionDto);
            }
        }
        public async Task<int> DeleteAsync(string userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query =
                    @"DELETE
                      FROM public.Subscribers  
                      WHERE user_id = @userId";
                return await connection.ExecuteAsync(query, new { userId });
            }
        }

        public  async Task<IEnumerable<SubscriptionDto>> GetFieldsForSearchAsync() 
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"
            SELECT 
                ""user_id"" AS UserId,
                ""name_product"" AS Name,
                ""price"" AS Price
            FROM public.Subscribers";
                return await connection.QueryAsync<SubscriptionDto>(query);
            }
        }
    }
}

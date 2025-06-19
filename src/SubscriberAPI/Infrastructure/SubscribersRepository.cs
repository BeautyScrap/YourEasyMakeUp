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

        public async Task CreateAsync(Subscription subscription)                                                       
        {
            var dto = subscription.ToDto();
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            var transaction = await connection.BeginTransactionAsync();
            try
            {
                string insertUserQuery =
                    @"INSERT INTO Subscribers (user_id, chat_id, brand_product, name_product, price, url, status, created_at)
                    VALUES (@UserId, @ChatId, @Brand, @Name, @Price, @Url, @Status, NOW())
                    RETURNING id;";
                var subGuid = await connection.ExecuteScalarAsync<Guid>(insertUserQuery, dto, transaction);

                string insertStatusesQuery =
                    @"INSERT INTO sub_statuses(user_id,  status,  created_at) 
                    VALUES(@UserId, @Status, NOW())";
                await connection.ExecuteAsync(insertStatusesQuery, new 
                {
                    UserId = subGuid,
                    Status = dto.Status
                }, transaction);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;

            }
        }       

        public async Task<IEnumerable<Subscription>> GetAllSubscribersAsync()
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
                var dtos = await connection.QueryAsync<SubscriptionDto>(query);
                var subscriptions = dtos.Select(x => Subscription.CreateNewSubscription(
                    x.UserId,
                    x.ChatId,
                    x.Brand,
                    x.Name,
                    x.Price
                )).ToList();
                return subscriptions;
            }
        }
        public async Task<Subscription> GetSubscriberAsync(string userId)
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
                var subDto = await connection.QuerySingleOrDefaultAsync<SubscriptionDto>(query, new { userId });
                var result = Subscription.CreateNewSubscription(
                    userId,
                    subDto.ChatId,
                    subDto.Brand,
                    subDto.Name,
                    subDto.Price);
                return result;
            }
        }
        public async Task<int> UpdateAsync(Subscription subscription)
        {
            var dto = subscription.ToDto();
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            var transaction = await connection.BeginTransactionAsync();
            try
            {
                string query =
                    @"UPDATE public.Subscribers 
                      SET user_id = @UserId, 
                            chat_id = @ChatId, 
                            brand_product = @Brand, 
                            name_product = @Name, 
                            price = @Price, 
                            url = @Url,
                            status = @Status
                      WHERE id = (
                                SELECT id FROM public.Subscribers
                                WHERE userid = @UserId
                                ORDER BY created_at DESC
                                LIMIT 1 )
                            RETURNING id;";
                var userGuid = await connection.ExecuteScalarAsync<Guid>(query, dto, transaction);
                var insertStatusQuery =
                    @"INSERT INTO sub_statuses(user_id,  status,  created_at) 
                    VALUES(@UserId, @Status, NOW())";
                var result = await connection.ExecuteAsync(insertStatusQuery, new
                {
                    UserId = userGuid,
                    Status = dto.Status
                },
                transaction);
                await transaction.CommitAsync();
                return result;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
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

        public async Task<IEnumerable<Subscription>> GetFieldsForSearchAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query =
                    @"SELECT 
                ""user_id"" AS UserId,
                ""name_product"" AS Name,
                ""price"" AS Price
                   FROM public.Subscribers
                   WHERE status = 0";
                var subDto = await connection.QueryAsync<Subscription>(query);
                var listResult = subDto.Select(dto => Subscription.CreateNewSubscription(
                dto.UserId,
                dto.ChatId,
                dto.Brand,
                dto.Name,
                dto.Price
                )).ToList();
                return listResult;
            }
        }

        public async Task<int> UpdateStatusFoundProduct(string userId, string name)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            string query =
                @"UPDATE public.Subscribers
                        SET status = @Status
                        WHERE user_id = @UserId AND name_product = @Name";
            var result = await connection.ExecuteAsync(query, new
            {
                UserId = userId,
                Name = name,
                Status = 1
            });
            return result;
        }
    }
}

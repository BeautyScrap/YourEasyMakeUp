using Dapper;
using Npgsql;
using System.Transactions;
using Telegram.Bot.Types;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Entities;
using YourEasyRent.UserState;

namespace YourEasyRent.DataBase
{
    public class UserStateRepository : IUserStateRepository
    {

        private readonly string _connectionString;
        public UserStateRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task CreateAsync(UserSearchState userSearchState)
        {
            var dto = userSearchState.ToDto();
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            var transaction = await connection.BeginTransactionAsync();
            try
            {
                string inserUserQuery =
                @"INSERT INTO telegramchats (userid, chatid, category, brand, menu_status, name, price, created_at) 
                    VALUES (@UserId, @ChatId, @Category, @Brand, @Menu_status, @Name, @Price, NOW())
                    RETURNING id;";
                var userGuid = await connection.ExecuteScalarAsync<Guid>(inserUserQuery, dto, transaction);

                string insertMenuStatusesQuery =
                    @"INSERT INTO menu_statuses(user_id,  menu_status,  created_at)
                    VALUES(@UserId, @Menu_status, NOW())";
                await connection.ExecuteAsync(insertMenuStatusesQuery, new
                {
                    UserId = userGuid,
                    Menu_status = dto.Menu_status
                }, transaction);

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<bool> UpdateAsync(UserSearchState userSearchState)// LAST Update  все протестировала, надо протеастировать как сохраняется подписчик в другой сервис и оповещение об изменении цены
        {
            var dto = userSearchState.ToDto();
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            var transaction = await connection.BeginTransactionAsync();
            try
            {
                var updateUserQuery = @"
                            UPDATE telegramchats
                            SET 
                                chatid = @ChatId, 
                                category = @Category, 
                                brand = @Brand,
                                menu_status = @Menu_status, 
                                name = @Name,
                                price = @Price
                            WHERE id = (
                                SELECT id FROM telegramchats
                                WHERE userid = @UserId
                                ORDER BY created_at DESC
                                LIMIT 1 )
                            RETURNING id;";
                var userGuid = await connection.ExecuteScalarAsync<Guid>(updateUserQuery, dto, transaction);
                var insertStatusQuery =
                    @"INSERT INTO menu_statuses(user_id,  menu_status,  created_at)
                    VALUES(@UserId, @Menu_status, NOW())";

                var result = await connection.ExecuteAsync(insertStatusQuery, new
                {
                    UserId = userGuid,
                    Menu_status = dto.Menu_status
                }, transaction);
                await transaction.CommitAsync();
                return result > 0;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }



        public async Task<UserSearchState> GetForUser(string userId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            string query = @"
                    SELECT 
                userid AS UserId,
                chatid AS ChatId, 
                category AS Category, 
                brand AS Brand,
                menu_status AS Menu_status, 
                name AS Name,
                price AS Price
                    FROM telegramchats
                    WHERE userid = @UserId
                    ORDER BY created_at DESC
                    LIMIT 1;
                            ";  
            var resultDto = await connection.QueryFirstOrDefaultAsync<UserSearchStateDTO>(query, new { UserId = userId });
            var userSearchState = UserSearchState.FromDto(userId, resultDto);
            return userSearchState;

        }
        public async Task<(string Brand, string Category)> GetBrandAndCategoryForSearch(string userId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            var query = @"SELECT brand, category
                      FROM telegramchats
                      WHERE userid = @UserId
                      ORDER BY created_at DESC
                      LIMIT 1";
            var resultBrandCategory = await connection.QueryFirstOrDefaultAsync<(string, string)>(query, new { UserId = userId });
            return resultBrandCategory;
        }

        public Task<MenuStatus> GetCurrentStateForUser(string userId) // эти 2 метода вроде как и не нужны
        {
            throw new NotImplementedException();
        }
        public Task<bool> CheckFieldsFilledForUser(string userId)
        {
            throw new NotImplementedException();
        }
    }
}

//        public async Task<UserSearchState> GetForUser(string userId)
//        {
//            var filter = Builders<UserSearchStateDTO>.Filter.Eq(u => u.UserId, userId);

//            var dto = await _collectionOfUserSearchState.Find(filter).FirstOrDefaultAsync();

//            var state = new UserSearchState(dto);

//            return state;
//        }

//        public async Task CreateAsync(UserSearchState userSearchState)
//        {
//            var dto = userSearchState.ToDto();
//            await _collectionOfUserSearchState.InsertOneAsync(dto);
//        }

//        public async Task<bool> UpdateAsync(UserSearchState userSearchState)
//        {
//            var filter = Builders<UserSearchStateDTO>.Filter.Eq(u => u.UserId, userSearchState.UserId);
//            var update = Builders<UserSearchStateDTO>.Update
//                .Set(u => u.UserId, userSearchState.UserId)
//                .Set(u => u.Brand, userSearchState.Brand)
//                .Set(u => u.Category, userSearchState.Category)
//                .Set(u => u.Status, userSearchState.CurrentMenuStatus)
//                .Set(u => u.HistoryOfMenuStatuses, userSearchState.HistoryOfMenuStatuses)
//                .Set(u => u.Name, userSearchState.Name)
//                .Set(u => u.Price, userSearchState.Price);

//            var updateResult = await _collectionOfUserSearchState.UpdateOneAsync(filter, update);
//            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
//        }

//        public async Task<MenuStatus> GetCurrentStateForUser(string userId)
//        {
//            var filter = Builders<UserSearchStateDTO>.Filter.Eq(u =>u.UserId, userId);
//            var dto = await _collectionOfUserSearchState.Find(filter).FirstOrDefaultAsync();

//            var state = new UserSearchState(dto);

//            return state.CurrentMenuStatus;

//        }

//        public async Task<bool> CheckFieldsFilledForUser(string userId)
//        {
//            var filter = Builders<UserSearchStateDTO>.Filter.Eq(u => u.UserId, userId);
//            var dto = await _collectionOfUserSearchState.Find(filter).FirstOrDefaultAsync();

//            if (dto == null)
//            {
//                return false;
//            }
//            return true;

//        }

//        public async Task<(string Brand, string Category)> GetBrandAndCategoryForSearch(string userId)
//        {
//            var filter = Builders<UserSearchStateDTO>.Filter.Eq(u => u.UserId, userId);
//            var projection = Builders<UserSearchStateDTO>.Projection.Include(u =>u.Brand).Include(u => u.Category);
//            var brandAndCategoryResult = await _collectionOfUserSearchState.Find(filter).Project(u =>new { u.Brand, u.Category} ).FirstOrDefaultAsync();

//            return (brandAndCategoryResult.Brand, brandAndCategoryResult.Category);
//        }

//    }
//}

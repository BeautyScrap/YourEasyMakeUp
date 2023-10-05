using MongoDB;
using MongoDB.Driver;
using Telegram.Bot;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Entities;

namespace YourEasyRent.DataBase
{
    public class TelegramRepository : ITelegramRepository
    {
        public async Task<IEnumerable<Product>> GetProductsByBrandAndPrice(string brand, decimal price)
        {
            
        }
    }
}

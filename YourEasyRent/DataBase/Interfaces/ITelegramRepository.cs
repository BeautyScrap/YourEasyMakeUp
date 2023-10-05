using MongoDB;
using YourEasyRent.Entities;

namespace YourEasyRent.DataBase.Interfaces
{
    public interface ITelegramRepository
    {
        Task<IEnumerable<Product?>> GetProductsByBrandAndPrice(string brand, decimal price);
    }
}

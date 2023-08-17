using YourEasyRent.Entities;
using YourEasyRent.DataBase;
using MongoDB.Driver;
using System;
using YourEasyRent.Repositories.Interfaces;

namespace YourEasyRent.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public Task Create(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> CreateMany(IEnumerable<Product> products)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<Product> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetByBrand(string brand)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetByCategory(string category)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProducts()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Product product)
        {
            throw new NotImplementedException();
        }
    }
}

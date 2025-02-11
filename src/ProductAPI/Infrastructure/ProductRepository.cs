using Dapper;
using Npgsql;
using ProductAPI.Domain.Product;
using ProductAPI.Domain.ProductForUser;
using System.Xml.Linq;
using System;

namespace ProductAPI.Infrastructure
{
    public class ProductRepository : IProductRepository
    {
        private readonly ILogger<ProductRepository> _logger;
        private readonly string _connectionString;
        public ProductRepository(string connectionString, ILogger<ProductRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }
        public async Task CreateMany(IEnumerable<Product> products) // insert добавляет строки в таблицу
        {
            foreach (var product in products)
            {
                var dto = product.ToDto();
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query =
                        @"INSERT INTO products (site, brand, name, price, category, url, imageurl) 
                    VALUES (@Site, @Brand, @Name, @Price, @Category, @Url, @ImageUrl)";
                    await connection.ExecuteAsync(query, dto);
                }
            }
        }


        public async Task<int> UpdateManyProducts(IEnumerable<Product> products)// AK TODO  номальный ли метод получился с такими уровнями изоляции транзакций?
        {
            int totalUpdated = 0;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        foreach (var product in products)
                        {
                            var dto = product.ToDto();
                            string query =
                                    @"UPDATE public.products
                                    SET site = @Site, 
                                        brand = @Brand, 
                                        name = @Name, 
                                        price = @Price, 
                                        category = @Category, 
                                        url = @Url,
                                        imageurl = @Imageurl
                                    WHERE name = @Name";
                            int result = await connection.ExecuteAsync(query, dto);
                            totalUpdated += result;
                        };
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw ex;
                    }
                }
            }
            return totalUpdated;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            IEnumerable<ProductDto> productDtos;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"
            SELECT 
                ""site"" AS Site,
                ""brand"" AS Brand,
                ""name"" AS Name,
                ""price"" AS Price,
                ""category"" AS Category,
                ""url"" AS Url,
                ""imageurl"" AS Imageurl
            FROM public.products";
                productDtos = await connection.QueryAsync<ProductDto>(query);
            }
            var products = productDtos.Select(dto => Product.CreateProduct
                (
                dto.Site.Value,
                dto.Brand,
                dto.Name,
                dto.Price,
                dto.Category,
                dto.Url,
                dto.ImageUrl
                )
            ).ToList();
            return products;
        }

        public async Task<int> Delete(string name)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query =
                    @"DELETE
                      FROM public.products  
                      WHERE name = @Name";
                return await connection.ExecuteAsync(query, new {name});
            }
        }
        public async Task<List<string>> GetBrands()
        {
            using (var connnection = new NpgsqlConnection(_connectionString))
            {
                await connnection.OpenAsync();
                string query =
                    @"SELECT DISTINCT ""brand""
                      FROM public.products 
                      LIMIT 5";
                var brand = await connnection.QueryAsync<string>(query);
                return brand.ToList();
            }
        }

        public async Task<AvaliableResultForUser?> GetOneProductResultForUser(ProductResultForUser productForUser)
        {
            var dto = productForUser.ToDto();
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            string query =
                    @"SELECT ""brand"" AS Brand,
                             ""name"" AS Name,
                             ""price"" AS Price,
                             ""category"" AS Category,
                             ""url"" AS Url,
                             ""imageurl"" AS Imageurl
                      FROM public.products
                      WHERE brand = @Brand AND category = @Category
                      LIMIT 1";
            var resultDto = await connection.QueryFirstOrDefaultAsync<AvaliableResultForUserDto>(query, dto);
            return resultDto is not null ? AvaliableResultForUser.FromDto(resultDto): null; 
        }


        //public async Task<Product> Get(string id)
        //{
        //    return await _productCollection.Find(_ => _.Id == id).FirstOrDefaultAsync();
        //}

        //public async Task<IEnumerable<Product>> GetByBrand(string brand)
        //{
        //    return await _productCollection.Find(_ => _.Brand == brand).ToListAsync();
        //}

        //public async Task<Product> GetByName(string name)
        //{
        //    return await _productCollection.Find(_ => _.Name == name).FirstOrDefaultAsync();
        //}

        //public async Task Create(Product newproduct)
        //{
        //    await _productCollection.InsertOneAsync(newproduct);
        //}

        //public async Task CreateMany(IEnumerable<Product> products)
        //{
        //    await _productCollection.InsertManyAsync(products);
        //    await DeleteDuplicate();


        //}

        //public async Task<bool> Update(Product updateProduct)
        //{
        //    var filter = Builders<Product>.Filter.And(
        //        Builders<Product>.Filter.Eq(_ => _.Brand, updateProduct.Brand),
        //        Builders<Product>.Filter.Eq(_ => _.Name, updateProduct.Name));
        //    var updateResult = await _productCollection.ReplaceOneAsync(filter, updateProduct);
        //    return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        //}



        //public async Task UpsertProduct(Product product)
        //{
        //    var filter = Builders<Product>.Filter.And
        //        (Builders<Product>.Filter.Eq(_ => _.Brand, product.Brand),
        //        Builders<Product>.Filter.Eq(_ => _.Name, product.Name));

        //    var update = Builders<Product>.Update
        //        .Set(_ => _.Price, product.Price)
        //        .Set(_ => _.Url, product.Url);

        //    var options = new UpdateOptions
        //    {
        //        IsUpsert = true
        //    };
        //    await _productCollection.UpdateOneAsync(filter, update, options);
        //    await DeleteDuplicate();
        //}

        //public async Task UpsertManyProducts(IEnumerable<Product> products)
        //{
        //    foreach (var product in products)
        //    {
        //        await UpsertProduct(product);
        //    }
        //}


        //public async Task<AvaliableProductDto?> GetProductForOneSubscriber(ProductForSubDto productForSearch)
        //{
        //    double roundedPrice = Math.Round((double)productForSearch.Price, 2);
        //    var filter = Builders<Product>.Filter.And(
        //            Builders<Product>.Filter.Eq(_ => _.Name, productForSearch.Name),
        //            Builders<Product>.Filter.Lt(_ => (double)_.Price, roundedPrice));
        //    var product = await _productCollection.Find(filter).FirstOrDefaultAsync();
        //    if (product == null)
        //    {
        //        return null;
        //    }
        //    var result = new AvaliableProductDto()
        //    {
        //        Brand = product.Brand,
        //        Name = product.Name,
        //        Price = product.Price,
        //        Url = product.Url,
        //        UrlImage = product.ImageUrl
        //    };
        //    return result;
        //}

        //public async Task<IEnumerable<AvaliableResultForUserDto>> GetProductResultForUser(ProductResultForUserDto productForUser)
        //{
        //    var filter = Builders<Product>.Filter.And(
        //        Builders<Product>.Filter.Eq(_ => _.Brand, productForUser.Brand),
        //        Builders<Product>.Filter.Eq(_ => _.Category, productForUser.Category));
        //    var products = await _productCollection.Find(filter).ToListAsync();
        //    var results = new List<AvaliableResultForUserDto>();
        //    foreach (var product in products)
        //    {
        //        var productResult = new AvaliableResultForUserDto()
        //        {
        //            Brand = product.Brand,
        //            Name = product.Name,
        //            Category = product.Category,
        //            Price = product.Price,
        //            ImageUrl = product.ImageUrl,
        //            Url = product.Url
        //        };
        //        results.Add(productResult);
        //    }
        //    return results;
        //}

        //public async Task DeleteDuplicate()
        //{
        //    var groups = await _productCollection.Aggregate()
        //        .Group(p => new { p.Name }, g => new
        //        {
        //            Name = g.Key,
        //            Count = g.Count(),
        //            Ids = g.Select(x => x.Id).ToList()
        //        }).ToListAsync();
        //    var duplicateGroups = groups.Where(g => g.Count > 1);
        //    foreach (var group in duplicateGroups)
        //    {
        //        var idsToDelete = group.Ids.Skip(1).ToList();
        //        var filter = Builders<Product>.Filter.In(p => p.Id, idsToDelete);
        //        await _productCollection.DeleteManyAsync(filter);
        //    }
        //}

        //public async Task<AvaliableResultForUserDto> GetOneProductResultForUser(ProductResultForUserDto productForUser) // использую этот метод
        //{
        //    var filter = Builders<Product>.Filter.And(
        //        Builders<Product>.Filter.Eq(_ => _.Brand, productForUser.Brand),
        //        Builders<Product>.Filter.Eq(_ => _.Category, productForUser.Category));
        //    var product = await _productCollection.Find(filter).FirstOrDefaultAsync();
        //    var productResult = new AvaliableResultForUserDto()
        //    {
        //        Brand = product.Brand,
        //        Name = product.Name,
        //        Category = product.Category,
        //        Price = product.Price,
        //        ImageUrl = product.ImageUrl,
        //        Url = product.Url
        //    };
        //    return productResult;
        //}
    }
}

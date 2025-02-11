using MongoDB.Driver;
using ProductAPI.Domain.ProductForUser;
using ProductAPI.Infrastructure;
using ZstdSharp.Unsafe;

namespace ProductAPI.Application
{
    public class ProductForUserService:IProductForUserService
    {
        private readonly IProductRepository _repository;
        public ProductForUserService(IProductRepository productRepository)
        {
            _repository = productRepository;
        }

        public async Task<List<AvaliableResultForUser>> Handler(ProductResultForUser product)// этот метод пока не использую
        {
            //var productDto = product.ToDto();
            //var resultProductDtos = await _productRepository.GetProductResultForUser(productDto);
            //var result = new List<AvaliableResultForUser>();
            //foreach (var resultDto in resultProductDtos)
            //{
            //    AvaliableResultForUser avaliableResult = AvaliableResultForUser.FromDto(resultDto);
            //    result.Add(avaliableResult);   
            //}
            //return result;
            throw new NotImplementedException();
        }

        public async Task<AvaliableResultForUser?> HandlerOne(ProductResultForUser product) // использую этот метод
        {          
            var resultProduct = await _repository.GetOneProductResultForUser(product);
            return resultProduct; 
        }

       public async Task<List<string>> GetBrandForMenu()
        {
            var result = await _repository.GetBrands();
            return result;
        }
    }
}

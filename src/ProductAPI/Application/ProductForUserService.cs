using MongoDB.Driver;
using ProductAPI.Domain.ProductForUser;
using ProductAPI.Infrastructure;

namespace ProductAPI.Application
{
    public class ProductForUserService:IProductForUserService
    {
        private readonly IProductRepository _productRepository;
        public ProductForUserService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<List<AvaliableResultForUser>> Handler(ProductResultForUser product)
        {
            var productDto = product.ToDto();
            var resultProductDtos = await _productRepository.GetProductResultForUser(productDto);
            var result = new List<AvaliableResultForUser>();
            foreach (var resultDto in resultProductDtos)
            {
                AvaliableResultForUser avaliableResult = AvaliableResultForUser.FromDto(resultDto);
                result.Add(avaliableResult);   
            }
            return result;
        }

        public async Task<AvaliableResultForUser> HandlerOne(ProductResultForUser product) 
        {
            var productDto = product.ToDto();
            var resultProductDtos = await _productRepository.GetOneProductResultForUser(productDto);
            AvaliableResultForUser avaliableResult = AvaliableResultForUser.FromDto(resultProductDtos);
            return avaliableResult;
        }
    }
}

using ProductAPI.Domain.ProductForSubscription;
using ProductAPI.Infrastructure;

namespace ProductAPI.Application
{
    public class ProductForSubService : IProductForSubService
    {
        private readonly IProductRepository _productRepository;

        public ProductForSubService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<List<AvaliableProduct>> ProductForSubHandler(List<ProductForSub> products)
        {
            //var listWithProducts = new List<AvaliableProduct>();

            //foreach (var product in products)
            //{
            //    var productDto = product.ToDto();
            //    var userId = productDto.UserId;

            //    var resultProductDto = await _productRepository.GetProductForOneSubscriber(productDto);
            //    if (resultProductDto == null)
            //    {
            //        continue;
            //    }
            //    AvaliableProduct avaliableProduct = AvaliableProduct.FromDto(userId, resultProductDto);
            //    listWithProducts.Add(avaliableProduct); 
            //}
            //return listWithProducts;
            throw new NotImplementedException();
        }
    }
}

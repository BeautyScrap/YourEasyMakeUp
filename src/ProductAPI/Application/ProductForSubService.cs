using ProductAPI.Domain.ProductForSubscription;
using ProductAPI.Infrastructure;

namespace ProductAPI.Application
{
    public class ProductForSubService : IProductForSubService
    {
        private readonly IProductRepository _repository;

        public ProductForSubService(IProductRepository productRepository)
        {
            _repository = productRepository;
        }
        public async Task<List<AvaliableProduct>> ProductForSubHandler(List<ProductForSub> products)
        {
            var listWithProducts = new List<AvaliableProduct>();

            foreach (var product in products)
            {
                var userId = product.UserId;
                var foundProduct = await _repository.GetProductForOneSubscriber(userId,product);
                if (foundProduct == null)
                {
                    continue;
                }
                listWithProducts.Add(foundProduct);
            }
            return listWithProducts;
        }
            //    var productDto = product.ToDto();
            //    var userId = productDto.UserId;

            //    var resultProductDto = await _productRepository.GetProductForOneSubscriber(productDto);
            //    AvaliableProduct avaliableProduct = AvaliableProduct.FromDto(userId, resultProductDto);
            //    listWithProducts.Add(avaliableProduct); 
            //}
            //return listWithProducts;
    }
}

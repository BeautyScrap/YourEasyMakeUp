using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application;
using ProductAPI.Contracts.ProductForSubscription;
using ProductAPI.Contracts.TelegramContract;
using ProductAPI.Domain.Product;
using ProductAPI.Domain.ProductForSubscription;
using ProductAPI.Domain.ProductForUser;
using ProductAPI.Infrastructure;
using ProductAPI.Infrastructure.Client;
using SubscriberAPI.Application.RabbitQM;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("")]
    public class ProductController : ControllerBase 
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ProductController> _logger;
        private readonly IRabbitMessageProducer _messageProducer;
        private readonly IProductForSubService _serviceForSub;
        private readonly IProductForUserService _serviceForUse; 
        public ProductController(
            IProductRepository productRepository, 
            ILogger<ProductController> logger,
            IRabbitMessageProducer rabbitMessage, 
            IProductForSubService serviceSub,

            IProductForUserService serviceProduct)
        {
            _repository = productRepository;
            _logger = logger;
            _messageProducer = rabbitMessage;
            _serviceForSub = serviceSub;
            _serviceForUse = serviceProduct;
        }

        [HttpPost]
        [Route("SearchProductsResultForUser")] // этим методом пока не пользуюсь
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FoundProductResultResponse>>> Search([FromBody] SearchProductResultRequest request)
        {
            try 
            {
                if(request == null) 
                {
                    return BadRequest();
                }
                var searchProducts = ProductResultForUser.CreateProductForSearch(
                    request.Brand, 
                    request.Category);
                var foundProductList = await _serviceForUse.Handler(searchProducts);
                if(foundProductList == null) 
                { 
                    return NotFound(); 
                }
                var response = foundProductList.Select(r => new FoundProductResultResponse
                {
                    Brand = r.Brand,
                    Name = r.Name,
                    Category = r.Category,
                    Price = r.Price,
                    ImageUrl = r.ImageUrl,
                    Url = r.Url

                }).ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Post]: Failed to check the SearchProductsResultForUser");
                return StatusCode(500, "Failed to check the SearchProductResultRequest");
            }        
        }

        [HttpPost]
        [Route("SearchOneProductResultForUser")] // Остановилась пока на этом методе с одним результатом, тк не придумала как отслеживать потом несколько результатов в телеграмме
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FoundProductResultResponse>>> SearchOneProductForUser([FromBody] SearchProductResultRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest();
                }
                var searchProducts = ProductResultForUser.CreateProductForSearch(
                    request.Brand,
                    request.Category);
                var foundProduct = await _serviceForUse.HandlerOne(searchProducts);
                if (foundProduct == null)
                {
                    return NotFound();
                }
                var response = new FoundProductResultResponse
                {
                    Brand = foundProduct.Brand,
                    Name = foundProduct.Name,
                    Category = foundProduct.Category,
                    Price = foundProduct.Price,
                    ImageUrl = foundProduct.ImageUrl,
                    Url = foundProduct.Url

                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Post]: Failed to check the SearchProductsResultForUser");
                return StatusCode(500, "Failed to check the SearchProductResultRequest");
            }

        }

        [HttpPost]
        [Route("SearchProductForSubscriber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FoundProductForSubResponse>>> Search([FromBody] List<SearchProductForSubRequest> productRequest)
        {
            try
            {
                //_messageProducer.ConsumingSubscriberMessage(productRequest);
                //if (productRequest is null)
                //{
                //    return BadRequest();
                //}
                var products = new List<ProductForSub>();
                foreach (var product in productRequest)
                {
                    var productForSubscription = ProductForSub.CreateProductForSearch(
                        product.UserId,
                        product.Name,
                        product.Price);

                    products.Add(productForSubscription);
                }
                var foundProductsList = await _serviceForSub.ProductHandler(products);
                var response = foundProductsList.Select(r => new FoundProductForSubResponse
                {
                    UserId = r.UserId,
                    Brand = r.Brand,
                    Name = r.Name,
                    Price = r.Price,
                    Url = r.Url,
                    UrlImage = r.UrlImage,

                }).ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Post]: Failed to check the SearchProductForSubscriber");
                return StatusCode(500, "Failed to check the SubscribersProductRequest");
            }
        }


        [Route("SearchBrands", Name = "SearchBrands")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FoundBrandForTelegramResponse>>> SearchBrands()
        {
            try
            {
                var brands = await _repository.GetBrandForMenu();
                if (brands == null) { return NotFound(); }
                var response = brands.Select(b => new FoundBrandForTelegramResponse() { Brand = b}).ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[SearchBrands]:Failed to found brand");
                return StatusCode(500, "InternalServerError");
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using YourEasyRent.DataBase;
using YourEasyRent.Entities;
using YourEasyRent.DataBase.Interfaces;
using Telegram.Bot.Types;
using Serilog;
using System.Xml.Linq;
using YourEasyRent.Services;
using YourEasyRent.Contracts.ProductForSubscription;
using YourEasyRent.Entities.ProductForSubscription;

namespace YourEasyRent.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ProductController> _logger;
        private readonly IRabbitMessageProducer _messageProducer;
        private readonly IProductForSubscriptionService _service;

        public ProductController(IProductRepository productRepository, ILogger<ProductController> logger,IRabbitMessageProducer rabbitMessage, IProductForSubscriptionService service)
        {
            _repository = productRepository;
            _logger = logger;
            _messageProducer = rabbitMessage;
            _service = service;
        }

        [HttpPost]
        [Route("SearchProductForSubscriber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ProductForSubscriptionResponce>>> Search([FromBody] List<ProductForSubscriptionRequest> productRequest)
        {
            try
            {
                _messageProducer.ConsumingSubscriberMessage(productRequest);
                if (productRequest is null)
                {
                    return BadRequest();
                }
                var products = new List<ProductForSubscription>();
                foreach (var product in productRequest)
                {
                    var productForSubscription = ProductForSubscription.CreateProductForSearch(
                        product.UserId,
                        product.ChatId,
                        product.Brand,
                        product.Name,
                        product.Price,
                        product.Url); //  надо проверить не выдет ли ошибку при создании объекта, те урла тут не передается

                    products.Add(productForSubscription);
                }
                var foundProductsList =  await _service.ProductHandler(products);
                var response = foundProductsList.Select(r => new ProductForSubscriptionResponce
                { 
                    UserId = r.UserId,
                    ChatId = r.ChatId,
                    Brand = r.Brand,
                    Name = r.Name,
                    Price = r.Price,
                    Url =  r.Url
                   
                }).ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Post]: Failed to check the SubscribersProductRequest");
                return StatusCode(500, "Failed to check the SubscribersProductRequest");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                var allProducts = await _repository.GetProducts();
                if (allProducts == null)
                {
                    return NotFound();
                }
                return allProducts.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[GetProducts] : An error occurred while processing the request");
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpGet("{id:length(24)}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> Get(string id)
        {
            try
            {
                var product = await _repository.Get(id);
                if (product == null)
                {
                    _logger.LogError($"Product with {id} is not found.");
                    return NotFound();
                }
                _logger.LogInformation("ActionResult = {@product}", product);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[GetProductById]: Product not found in the database");
                return StatusCode(500, "Internal Server Error. Product not found");
            }
        }

        [Route("[action]/{brand}", Name = "GetProductByBrand")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByBrand(string brand)
        {
            try
            {
                var brandProducts = await _repository.GetByBrand(brand);
                if (brandProducts == null)
                {
                    var result = Enumerable.Empty<Product>().ToList();
                    return Ok(result);
                }
                _logger.LogInformation("ActionResult = {@brand}", brand);
                return Ok(brandProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[GetProductByBrand]: Brand of Product is not found in the database");
                return StatusCode(404, "Brand not found");
            }
        }

        [Route("[action]/{name}", Name = "GetProductByName")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductByName(string name)
        {
            try
            {
                var nameProduct = await _repository.GetByName(name);
                _logger.LogInformation("ActionResult = {@name}", name);
                return Ok(nameProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[GetProductByName]: Name of Product is not found in the database");
                return StatusCode(404, "Unable to process the request. Name not found");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Post([FromBody] Product newProduct)
        {
            try
            {
                await _repository.Create(newProduct);
                if (newProduct != null)
                {
                    _logger.LogInformation("ActionResult = {@newProduct}", newProduct);
                    return CreatedAtAction(nameof(Get), new { id = newProduct.Id }, newProduct);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Post]: Failed to create user");
                return StatusCode(500, "Failed to create user.");
            }
        }

        [HttpPut("{id:length(24)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProduct(string id, Product updateProduct)
        {
            try
            {
                var product = await _repository.Get(id);
                if (product is null)
                {
                    _logger.LogInformation($"Product with this id {id} is not found");
                    return NotFound();
                }

                updateProduct.Id = product.Id;
                await _repository.Update(updateProduct);
                _logger.LogInformation("ActionResult = {@id} - product with this id is updated", id);
                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[UpdateProduct]: Failed to update user");
                return StatusCode(500, "Failed to update user.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                var product = await _repository.Get(id);
                if (product is null)
                {
                    _logger.LogInformation($"Product with this id {id} is not found");
                    return NotFound();
                }
                await _repository.Delete(id);
                _logger.LogInformation("ActionResult = {@id} - product with this id is deleted", id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[DeleteProduct]: Failed to delete user");
                return StatusCode(500, "Failed to delete user.");
            }
        }
    }
}

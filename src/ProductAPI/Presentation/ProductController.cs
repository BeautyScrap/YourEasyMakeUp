using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application;
using ProductAPI.Contracts.ProductForSubscription;
using ProductAPI.Domain.Product;
using ProductAPI.Domain.ProductForSubscription;
using ProductAPI.Infrastructure;
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
        private readonly IProductForSubService _service;
        private readonly IEnumerable<IProductsSiteClient> _siteClient;

        public ProductController(IProductRepository productRepository, ILogger<ProductController> logger, IRabbitMessageProducer rabbitMessage, IProductForSubService service, IEnumerable<IProductsSiteClient> siteClient)
        {
            _repository = productRepository;
            _logger = logger;
            _messageProducer = rabbitMessage;
            _service = service;
            _siteClient = siteClient;
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
                var foundProductsList = await _service.ProductHandler(products);
                var response = foundProductsList.Select(r => new FoundProductForSubResponse
                {
                    UserId = r.UserId,
                    Brand = r.Brand,
                    Name = r.Name,
                    Price = r.Price,
                    Url = r.Url

                }).ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Post]: Failed to check the SubscribersProductRequest");
                return StatusCode(500, "Failed to check the SubscribersProductRequest");
            }
        }

        [Route("GetAllProducts")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            try
            {
                var section = Section.Makeup;
                var allListings = new List<Product>();
                for (var pagenumber = 1; pagenumber < 4; pagenumber++)
                {
                    foreach (var client in _siteClient)
                    {
                        var products = await client.FetchFromSectionAndPage(section, pagenumber);
                        allListings.AddRange(products);
                    }
                }
                await _repository.UpsertManyProducts(allListings);
                return allListings;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[GetProductsListings] : An error occurred while processing the request");
                return (IEnumerable<Product>)StatusCode(500, "Internal Server Error.");

            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
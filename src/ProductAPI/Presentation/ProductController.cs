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
    public class ProductController : ControllerBase // AK TODO  разделить контроллер на 2 части- стандартные функции и которыми пользуюсь
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ProductController> _logger;
        private readonly IRabbitMessageProducer _messageProducer;
        private readonly IProductForSubService _serviceForSub;
        private readonly IEnumerable<IProductsSiteClient> _siteClient;
        private readonly IProductForUserService _serviceForUse; 
        public ProductController(
            IProductRepository productRepository, 
            ILogger<ProductController> logger,
            IRabbitMessageProducer rabbitMessage, 
            IProductForSubService serviceSub,
            IEnumerable<IProductsSiteClient> siteClient,
            IProductForUserService serviceProduct)
        {
            _repository = productRepository;
            _logger = logger;
            _messageProducer = rabbitMessage;
            _serviceForSub = serviceSub;
            _siteClient = siteClient;
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

        //[Route("UpsertAllProducts")] // удалить
        //[HttpPut]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IEnumerable<Product>> PutAllProducts()
        //{
        //    try
        //    {
        //        var section = Section.Makeup;
        //        var allListings = new List<Product>();
        //        for (var pagenumber = 1; pagenumber < 4; pagenumber++)
        //        {
        //            foreach (var client in _siteClient)
        //            {
        //                var products = await client.FetchFromSectionAndPage(section, pagenumber);
        //                allListings.AddRange(products);
        //            }
        //        }
        //        await _repository.UpsertManyProducts(allListings);
        //        return allListings;

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "[GetProductsListings] : An error occurred while processing the request");
        //        return (IEnumerable<Product>)StatusCode(500, "Internal Server Error.");

        //    }
        //}

        //[Route("CreateManyProducts")] // удалить
        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IEnumerable<Product>> CreateManyProducts() 
        //{
        //    try
        //    {
        //        var section = Section.Makeup;
        //        var allListings = new List<Product>();
        //        for (var pagenumber = 1; pagenumber < 4; pagenumber++)
        //        {
        //            foreach (var client in _siteClient)
        //            {
        //                var products = await client.FetchFromSectionAndPage(section, pagenumber);
        //                allListings.AddRange(products);
        //            }
        //        }
        //        await _repository.CreateMany(allListings);

        //        return allListings;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "[GetProductsListings] : An error occurred while processing the request");
        //        return (IEnumerable<Product>)StatusCode(500, "Internal Server Error.");

        //    }
        //}

        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        //{
        //    try
        //    {
        //        var allProducts = await _repository.GetProducts();
        //        if (allProducts == null)
        //        {
        //            return NotFound();
        //        }
        //        return allProducts.ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "[GetProducts] : An error occurred while processing the request");
        //        return StatusCode(500, "Internal Server Error.");
        //    }
        //}

        //[HttpGet("{id:length(24)}")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<Product>> Get(string id)
        //{
        //    try
        //    {
        //        var product = await _repository.Get(id);
        //        if (product == null)
        //        {
        //            _logger.LogError($"Product with {id} is not found.");
        //            return NotFound();
        //        }
        //        _logger.LogInformation("ActionResult = {@product}", product);
        //        return Ok(product);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "[GetProductById]: Product not found in the database");
        //        return StatusCode(500, "Internal Server Error. Product not found");
        //    }
        //}

        //[Route("[action]/{brand}", Name = "GetProductByBrand")]
        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<IEnumerable<Product>>> GetProductByBrand(string brand)
        //{
        //    try
        //    {
        //        var brandProducts = await _repository.GetByBrand(brand);
        //        if (brandProducts == null)
        //        {
        //            var result = Enumerable.Empty<Product>().ToList();
        //            return Ok(result);
        //        }
        //        _logger.LogInformation("ActionResult = {@brand}", brand);
        //        return Ok(brandProducts);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "[GetProductByBrand]: Brand of Product is not found in the database");
        //        return StatusCode(404, "Brand not found");
        //    }
        //}

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

        //[Route("[action]/{name}", Name = "GetProductByName")]
        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<Product>> GetProductByName(string name)
        //{
        //    try
        //    {
        //        var nameProduct = await _repository.GetByName(name);
        //        _logger.LogInformation("ActionResult = {@name}", name);
        //        return Ok(nameProduct);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "[GetProductByName]: Name of Product is not found in the database");
        //        return StatusCode(404, "Unable to process the request. Name not found");
        //    }
        //}

        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]

        //public async Task<IActionResult> Post([FromBody] Product newProduct)
        //{
        //    try
        //    {
        //        await _repository.Create(newProduct);
        //        if (newProduct != null)
        //        {
        //            _logger.LogInformation("ActionResult = {@newProduct}", newProduct);
        //            return CreatedAtAction(nameof(Get), new { id = newProduct.Id }, newProduct);
        //        }

        //        return BadRequest();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "[Post]: Failed to create user");
        //        return StatusCode(500, "Failed to create user.");
        //    }
        //}

        //[HttpPut("{id:length(24)}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> UpdateProduct(string id, Product updateProduct)
        //{
        //    try
        //    {
        //        var product = await _repository.Get(id);
        //        if (product is null)
        //        {
        //            _logger.LogInformation($"Product with this id {id} is not found");
        //            return NotFound();
        //        }

        //        updateProduct.Id = product.Id;
        //        await _repository.Update(updateProduct);
        //        _logger.LogInformation("ActionResult = {@id} - product with this id is updated", id);
        //        return Ok();

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "[UpdateProduct]: Failed to update user");
        //        return StatusCode(500, "Failed to update user.");
        //    }
        //}

        //[HttpDelete("{id}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> DeleteProduct(string id)
        //{
        //    try
        //    {
        //        var product = await _repository.Get(id);
        //        if (product is null)
        //        {
        //            _logger.LogInformation($"Product with this id {id} is not found");
        //            return NotFound();
        //        }
        //        await _repository.Delete(id);
        //        _logger.LogInformation("ActionResult = {@id} - product with this id is deleted", id);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "[DeleteProduct]: Failed to delete user");
        //        return StatusCode(500, "Failed to delete user.");
        //    }
        //}

        //[HttpDelete("DeleteDuplicates")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> DeleteDuplicates()
        //{
        //    try

        //    {
        //        await _repository.DeleteDuplicate();
        //        _logger.LogInformation("Dublicates are deleted");
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "[DeleteDuplicates]: Failed to delete Duplicates");
        //        return StatusCode(500, "Failed to delete ");
        //    }
        //}
    }
}
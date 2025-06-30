using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application;
using ProductAPI.Contracts;
using ProductAPI.Controllers;
using ProductAPI.Domain.Product;
using ProductAPI.Infrastructure;
using ProductAPI.Infrastructure.Client;

namespace ProductAPI.Presentation
{
    [ApiController]
    [Route("")]
    public class StandartMethodsController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IEnumerable<IProductsSiteClient> _siteClient;
        private readonly IProductHandler _handler;
        public StandartMethodsController(ILogger<ProductController> logger, IEnumerable<IProductsSiteClient> client, IProductHandler handler)
        {
            _logger = logger;
            _siteClient = client;
            _handler = handler;
        }

        [Route("CreateManyProducts")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CreateManyProductsResponse>>> CreateManyProducts() // использую только сефору
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
                await _handler.CreateManyProductAsync(allListings);
                var response = allListings.Select(p => new CreateManyProductsResponse
                {
                    Site = p.Site,
                    Brand = p.Brand,
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.Category,
                    Url = p.Url,
                    ImageUrl = p.ImageUrl
                }).ToList();
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[CreateManyProducts] : An error occurred while processing the request");
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [Route("UpsertAllProducts")] // UpdateManyProducts
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutAllProducts()
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
                var result = await _handler.UpsertManyProducts(allListings);
                if (result == false)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[GetProductsListings] : An error occurred while processing the request");
                return StatusCode(500, "Internal Server Error.");

            }
        }

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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                var allProducts = await _handler.GetAllProductsAsync();
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
        [HttpDelete("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(string name)
        {
            try
            {
                var result = await _handler.DeleteProductAsync(name);
                if (result is false)
                {
                    _logger.LogInformation($"Product with this name - {name} is not found");
                    return NotFound();
                }
                _logger.LogInformation($"Product with this name - {name} is deleted");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[DeleteProduct]: Failed to delete user");
                return StatusCode(500, "Failed to delete user.");
            }
        }


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

    }
}

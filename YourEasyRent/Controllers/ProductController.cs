using Microsoft.AspNetCore.Mvc;
using YourEasyRent.DataBase;
using YourEasyRent.Entities;
using YourEasyRent.DataBase.Interfaces;

namespace YourEasyRent.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")] // было [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository productRepository) => _repository = productRepository;


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()//   не надо бобавлять в скобках Id, потому что мы должне получить все продукты, поэтому и в операторах его нет Task<IEnumerable<Product>> GetProducts()
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
                Console.WriteLine($"[GetProducts] : {ex.Message}");
                return StatusCode(500, "Internal Server Error.");

            }
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Product>> Get(string id)
        {
            try
            {
                var product = await _repository.Get(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }

            catch (Exception ex) 
            {
                Console.WriteLine($"[Get] : {ex.Message}");
                return StatusCode(500, "Internal Server Error. Product not found");
            }
        }

        [Route("[action]/{brand}", Name = "GetProductByBrand")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByBrand(string brand)
        {
            try
            {
                var brandProducts = await _repository.GetByBrand(brand);
                if (brandProducts == null)
                {
                    return NotFound();
                }
                return Ok(brandProducts);
            }

            catch (Exception ex) 
            {
                Console.WriteLine($"[GetByBrand] : {ex.Message}");
                return StatusCode(500, "Internal Server Error. Brand not found");
            }
        }

        [Route("[action]/{name}", Name = "GetProductByName")]
        [HttpGet]
        public async Task<ActionResult<Product>> GetProductByName(string name)
        {
            try
            {
                var nameProduct = await _repository.GetByName(name);
                if (nameProduct == null)
                {
                    return NotFound();
                }
                return Ok(nameProduct);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"[GetByName] : {ex.Message}");
                return StatusCode(404, "Unable to process the request. Name not found");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Product newProduct)
        {
            try
            {
                await _repository.Create(newProduct);
                return CreatedAtAction(nameof(Get), new { id = newProduct.Id }, newProduct); 
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"[Create] : {ex.Message}");
                return StatusCode(500, "Failed to create user.");
            }
        }
        


        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateProduct(string id, Product updateProduct)
        {
            try
            {

                var product = await _repository.Get(id);

                if (product is null)
                {
                    return NotFound();
                }

                updateProduct.Id = product.Id;

                await _repository.Update(id, updateProduct);    

                return Ok();
                //var result = await _repository.Update(id, updateProduct);

                //if (result)
                //{
                //    return Ok();
                //}
                //return NotFound("Product not found or update failed.");
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"[Update] : {ex.Message}");
                return StatusCode(500, "Failed to update user.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {

                var result = await _repository.Delete(id);

                if (result)
                {
                    return Ok("Product deleted successfully.");
                }
                return NotFound("Product not found or delete failed.");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[Delete] : {ex.Message}");
                return StatusCode(500, "Failed to delete user.");
            }
        }







    }
}

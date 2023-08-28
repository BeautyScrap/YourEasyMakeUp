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
            var allProducts = await _repository.GetProducts();

            return Ok(allProducts); // получаем все продукты

        }
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Product>> Get(string id)
        {
            var product = await _repository.Get(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [Route("[action]/{brand}", Name = "GetProductByBrand")]
        [HttpGet]
        public async Task<ActionResult<Product>> GetProductByBrand(string brand)
        {
            var brandProduct = await _repository.GetByBrand(brand);
            if (brandProduct == null)
            {
                return NotFound();
            }
            return Ok(brandProduct);
        }
        [Route("[action]/{name}", Name = "GetProductByName")]
        [HttpGet]
        public async Task<ActionResult<Product>> GetProductByName(string name)
        {
            var nameProduct = await _repository.GetByName(name);
            if (nameProduct == null)
            {
                return NotFound();
            }
            return Ok(nameProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Product newProduct)
        {
            await _repository.Create(newProduct);
            return CreatedAtAction(nameof(Get), new { id = newProduct.Id }, newProduct);

        }
        


        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateProduct(string id, Product updateProduct)
        {
            var result = await _repository.Update(id, updateProduct);

            if (result)
            {
                return Ok("Product updated successfully.");
            }
            return NotFound("Product not found or update failed.");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var result = await _repository.Delete(id);

            if (result)
            {
                return Ok("Product deleted successfully.");
            }
            return NotFound("Product not found or delete failed.");
        }







    }
}

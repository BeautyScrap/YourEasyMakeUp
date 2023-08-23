using Microsoft.AspNetCore.Mvc;
using YourEasyRent.DataBase;
using YourEasyRent.Entities;
using YourEasyRent.DataBase.Interfaces;

namespace YourEasyRent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MongoController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public MongoController(IProductRepository productRepository) => _repository = productRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()//   не надо бобавлять в скобках Id, потому что мы должне получить все продукты, поэтому и в операторах его нет Task<IEnumerable<Product>> GetProducts()
        {
            var allProducts = await _repository.GetProducts();

            return Ok(allProducts); // получаем все продукты

        }
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Product>> GetById(string id)
        {
            var product = await _repository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("{brand:length(24)}")]
        public async Task<ActionResult<Product>> GetByBrand(string brand)
        {
            var brandProducts = await _repository.GetByBrand(brand);
            if (brandProducts == null)
            {
                return NotFound();
            }
            return Ok(brandProducts);
        }

        [HttpGet("{brand:length(24)}")]
        public async Task<ActionResult<Product>> GetByName(string name)
        {
            var nameProduct = await _repository.GetByName(name);
            if (nameProduct == null)
            {
                return NotFound();
            }
            return Ok(nameProduct);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post(Product newProduct)
        {
            await _repository.Create(newProduct);
            return CreatedAtAction(nameof(GetById), new { id = newProduct.Id }, newProduct);

        }
        [HttpPost]
        public async Task<IActionResult> CreateManyProducts(IEnumerable<Product> products)
        {
            if (products == null)
            {
                return BadRequest("Products data is missing.");
            }

            var createdIds = await _repository.CreateMany(products);
            return Ok(createdIds);
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

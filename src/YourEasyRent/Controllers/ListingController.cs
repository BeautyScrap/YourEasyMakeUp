using Microsoft.AspNetCore.Mvc;
using YourEasyRent.Entities;
using YourEasyRent.Services;
using YourEasyRent.DataBase.Interfaces;
using Section = YourEasyRent.Entities.Section;

namespace YourEasyRent.Controllers

{

    [ApiController]
    [Route("[controller]")] 
    public class ListingController : ControllerBase
    {
        private readonly IProductRepository _repository; 
        private readonly IEnumerable<IProductsSiteClient> _clients; 
        private ILogger<ListingController> _logger; 

        public ListingController(IProductRepository repository, IEnumerable<IProductsSiteClient> clients, ILogger<ListingController> logger)
        {
            _repository = repository;
            _clients = clients;
            _logger = logger;   
        }

        [HttpGet]
        [Route("products/")] 
        public async Task<IEnumerable<Product>> GetProducts()
        {
            try 
            { 
            var section = Section.Makeup;
            var allListings = new List<Product>();
            for (var pagenumber = 1; pagenumber < 4; pagenumber++)
            {
                foreach (var client in _clients)
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
    }
}



using HtmlAgilityPack;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using YourEasyRent.Contracts.ProductForSubscription;
using YourEasyRent.Entities.ProductForSubscription;
using YourEasyRent.Services;

namespace YourEasyRent.Controllers
{
    [Route("")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly IRabbitMessageProducer _messageProducer;
        private readonly ILogger<SubscriptionController> _logger;
        private readonly IProductForSubscriptionService _service;


        public SubscriptionController(IRabbitMessageProducer messageProducer, ILogger<SubscriptionController> logger, IProductForSubscriptionService service)
        {
            _messageProducer = messageProducer;
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Search([FromBody] List<SubscribersProductRequest> productRequest) 
        {
            try
            {
                _messageProducer.ConsumingSubscriberMessag(productRequest);
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
                        product.Price);

                    products.Add(productForSubscription);
                }
                await _service.ProductHandler(products);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Post]: Failed to check the SubscribersProductRequest");
                return StatusCode(500, "Failed to check the SubscribersProductRequest");
            }
        }

    }
}

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
        private readonly ProductForSubscriptionService _service;


        public SubscriptionController(IRabbitMessageProducer messageProducer, ILogger<SubscriptionController> logger, ProductForSubscriptionService service)
        {
            _messageProducer = messageProducer;
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Search([FromBody] SubscribersProductRequest request) 
        {
             _messageProducer.ConsumingSubscriberMessag(request);
            var products = new List<ProductForSubscription>();
            var product = ProductForSubscription.CreateProductForSearch
                        (request.UserId,
                         request.Name,
                         request.Brand,
                         request.Price);

                products.Add(product);
            await _service.ProductHandler(products);
            return Ok();
             // тут вызываем сервис, который берет этот лист и отправляет его в базу,  поэтому передаем в него аргумент products 
           //  такой длинный result потому что пока нет обращения в базу
        }

    }
}

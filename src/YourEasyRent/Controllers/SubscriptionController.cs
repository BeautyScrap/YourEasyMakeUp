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


        public SubscriptionController(IRabbitMessageProducer messageProducer, ILogger<SubscriptionController> logger)
        {
            _messageProducer = messageProducer;
            _logger = logger;
        }

        [HttpPost] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<ActionResult<List<ProductForSubscription>>> Search([FromBody] SubscribersProductRequest request) 
        {
             _messageProducer.ConsumingSubscriberMessag(request);
            var products = new List<ProductForSubscription>();

            {
                var product = ProductForSubscription.CreateProductForSearch
                        (request.UserId,
                         request.Name,
                         request.Brand,
                         request.Price);
                products.Add(product);
            } // тут вызываем сервис, который берет этот лист и отправляет его в базу
            return Task.FromResult<ActionResult<List<ProductForSubscription>>>(Ok(products)); //  такой длинный result потому что пока нет обращения в базу
        }

    }
}

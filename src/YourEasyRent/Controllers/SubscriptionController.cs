//using HtmlAgilityPack;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.AspNetCore.Mvc;
//using YourEasyRent.Contracts.ProductForSubscription;
//using YourEasyRent.Entities.ProductForSubscription;
//using YourEasyRent.Services;

//namespace YourEasyRent.Controllers
//{
//    [Route("")]
//    [ApiController]
//    public class SubscriptionController : ControllerBase
//    {
//        private readonly IRabbitMessageProducer _messageProducer;
//        private readonly IProductForSubscriptionService _service;
//        private readonly ILogger<SubscriptionController> _logger;


//        public SubscriptionController(IRabbitMessageProducer messageProducer, ILogger<SubscriptionController> logger, IProductForSubscriptionService service)
//        {
//            _messageProducer = messageProducer;
//            _logger = logger;
//            _service = service;
//        }

//        [HttpPost]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<IActionResult> Search([FromBody] List<SubscribersProductRequest> productRequest) //AK TODO возможно этот контроллер следует поместить в Product controller,
//                                                                                                           //потому что он от тоже работает с productRepository  и просто ищет нужный продукт в базе
//        {
//            try
//            {
//                _messageProducer.ConsumingSubscriberMessage(productRequest);
//                if (productRequest is null)
//                {
//                    return BadRequest();
//                }
//                var products = new List<ProductForSubscription>();
//                foreach (var product in productRequest)
//                {
//                    var productForSubscription = ProductForSubscription.CreateProductForSearch(
//                        product.UserId,
//                        product.ChatId,
//                        product.Brand,
//                        product.Name,
//                        product.Price,
//                        product.Url); //  надо проверить не выдет ли ошибку при создании объекта, те урла тут не передается

//                    products.Add(productForSubscription);
//                }
//                await _service.ProductHandler(products);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "[Post]: Failed to check the SubscribersProductRequest");
//                return StatusCode(500, "Failed to check the SubscribersProductRequest");
//            }
//        }

//    }
//}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YourEasyRent.Services;
using YourEasyRent.Entities;

namespace YourEasyRent.Controllers
{
    [Route("")] // api/[controller]
    [ApiController]
    public class SubscribersController : ControllerBase
    {
        private readonly IRabbitMessageProducer _messageProducer;
        private readonly ILogger<SubscribersController> _logger;
        //public static readonly List<Subscriber> _subscribers = new();

        public SubscribersController(IRabbitMessageProducer messageProducer, ILogger<SubscribersController> logger)
        {
            _messageProducer = messageProducer;
            _logger = logger;
        }

        [HttpPost]        
        public IActionResult SendSubscriber([FromBody] Subscriber newSubscriber)
        {
            if (newSubscriber == null) return BadRequest();
            //_subscribers.Add(newSubscriber);
            _messageProducer.SendMessagAboutSubscriber(newSubscriber);
            _logger.LogInformation("The message was sending(post)");
            return Ok();
        }
    }
}

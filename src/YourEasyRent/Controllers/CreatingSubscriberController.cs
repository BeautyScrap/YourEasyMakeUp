using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YourEasyRent.Services;
using YourEasyRent.Entities;

namespace YourEasyRent.Controllers
{
    [Route("")] // api/[controller]
    [ApiController]
    public class CreatingSubscriberController : ControllerBase
    {
        private readonly IRabbitMessageProducer _messageProducer;
        private readonly ILogger<CreatingSubscriberController> _logger;
        public static readonly List<Subscriber> _subscribers = new();

        public CreatingSubscriberController(IRabbitMessageProducer messageProducer, ILogger<CreatingSubscriberController> logger)
        {
            _messageProducer = messageProducer;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult CreateSubscriber(Subscriber newSubscriber)
        {
            if (!ModelState.IsValid) return BadRequest();
            _subscribers.Add(newSubscriber);
            _messageProducer.SendMessage<Subscriber>(newSubscriber);
            return Ok();
        }
    }
}

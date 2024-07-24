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
        [Route("subscriber/")]
        public IActionResult SendSubscriber([FromBody] Subscriber newSubscriber)
        {
            if (!ModelState.IsValid) return BadRequest();
            _subscribers.Add(newSubscriber);
            _messageProducer.SendMessagAboutSubscriber(newSubscriber);
            Console.WriteLine("The message was sending(post)");
            return Ok();
        }
    }
}

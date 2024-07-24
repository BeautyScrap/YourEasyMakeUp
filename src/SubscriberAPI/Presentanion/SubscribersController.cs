using Microsoft.AspNetCore.Mvc;
using SubscriberAPI.Application;
using SubscriberAPI.Domain;

namespace SubscriberAPI.Presentanion
{
    [ApiController]
    [Route("")]
    public class SubscribersController : ControllerBase
    {
        private readonly IRabbitMessageProducer _messageProducer;
        private readonly ILogger<SubscribersController> _logger;
        public static readonly List<Subscriber> _subscribers = new();

        public SubscribersController(IRabbitMessageProducer messageProducer, ILogger<SubscribersController> logger)
        {
            _messageProducer = messageProducer;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Post(Subscriber subscriber)
        {
            _messageProducer.ConsumingMessage(subscriber);
            _subscribers.Add(subscriber);
            Console.WriteLine($" The subscriber has been received to controller(get) {subscriber}");
            return Ok();
      
        }

    }
}
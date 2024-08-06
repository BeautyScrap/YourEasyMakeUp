using Microsoft.AspNetCore.Mvc;
using SubscriberAPI.Application;
using SubscriberAPI.Domain;
using SubscriberAPI.Infrastructure;
using System.Reflection.Metadata;

namespace SubscriberAPI.Presentanion
{
    [ApiController]
    [Route("")]
    public class SubscribersController : ControllerBase
    {
        private readonly IRabbitMessageProducer _messageProducer;
        private readonly ILogger<SubscribersController> _logger;
        //public static readonly List<Subscriber> _subscribers = new();
        public readonly ISubscribersRepository _subscribersRepository;

        public SubscribersController(IRabbitMessageProducer messageProducer, ILogger<SubscribersController> logger, ISubscribersRepository subscribersRepository)
        {
            _messageProducer = messageProducer;
            _logger = logger;
            _subscribersRepository = subscribersRepository;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] Subscriber subscriber)
        {
            try
            {
                if (subscriber == null)
                {
                    _logger.LogInformation("The subscriber is null");
                    return BadRequest();
                }
                _messageProducer.ConsumingMessage(subscriber);
                await _subscribersRepository.Create(subscriber);// после получения результата перекинуть его в хендлер для обработки этого подписчика, типо TgButtonCallback tgButtonCallback = new TgButtonCallback(update);await _handler.HandleUpdateAsync(tgButtonCallback); И тут влепить проверку на создавшийся экземпляр в бд
                Console.WriteLine($" The subscriber has been received to controller(get) {subscriber}");
                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Post]: Failed to create subscriber");
                return StatusCode(500, "Failed to create subscriber.");
            }

        }

    }
}
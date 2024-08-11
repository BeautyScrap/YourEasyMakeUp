using Microsoft.AspNetCore.Mvc;
using SubscriberAPI.Application;
using SubscriberAPI.Application.RabbitQM;
using SubscriberAPI.Domain;
using SubscriberAPI.Infrastructure;
using System.Reflection.Metadata;
using System.Text.Json;

namespace SubscriberAPI.Presentanion
{
    [ApiController]
    [Route("")]
    public class SubscribersController : ControllerBase
    {
        private readonly IRabbitMessageProducer _messageProducer;
        private readonly ILogger<SubscribersController> _logger;
        public readonly ISubscrieberService _sudscriberService;

        public SubscribersController(IRabbitMessageProducer messageProducer, ILogger<SubscribersController> logger, ISubscrieberService subscrieberService)
        {
            _messageProducer = messageProducer;
            _logger = logger;
            _sudscriberService  = subscrieberService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] SudscriberDto subscriberDto)
        {
            if (subscriberDto == null)
            {
                _logger.LogInformation("The subscriber is null");
                return BadRequest();
            }
            try
            {
                var subscriber = new Subscriber(subscriberDto);
                _messageProducer.ConsumingMessage(subscriber);
                await _sudscriberService.Create(subscriber);
                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Post]: Failed to create subscriber");
                return StatusCode(500, "Failed to create subscriber.");
            }

        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Subscriber>>> Get()
        {
            var allSubscribers = await _sudscriberService.GetAllAsync();
            if (allSubscribers == null)
            {
                return BadRequest(); 
            }
            return Ok(allSubscribers);
        }

    }


}
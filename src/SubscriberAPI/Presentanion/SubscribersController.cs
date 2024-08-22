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
            _sudscriberService = subscrieberService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] SubscriberDto subscriberDto)
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

        [Route("GetAllSubscribers")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            var allSubscribersDto = await _sudscriberService.GetAllAsync();
            if (allSubscribersDto == null)
            {
                return BadRequest();
            }
            return Ok(allSubscribersDto);
        }

        [Route("[action]/{userId}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(string userId)
        {
            var subscriber = await _sudscriberService.GetById(userId);
            if (subscriber == null)
            {
                return NotFound();
            }
            return Ok(subscriber);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] Subscriber subscriber, string userId)
        {
            var result = await _sudscriberService.Update(userId, subscriber);
            if(result ==  false)
            {
                return NotFound();
            }
            return Ok(result);        
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string userId)
        {
            var result = await _sudscriberService.Delete(userId);
            if (result == false)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [Route("GetFieldsForSearch")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<SubscriberDto>>> GetFieldsForSearch()
        {
            var listWithFields = await _sudscriberService.GetFieldsForSearchById();

            if (listWithFields == null)
            {
                return BadRequest();
            }
            return listWithFields.ToList();
        }
    }


}
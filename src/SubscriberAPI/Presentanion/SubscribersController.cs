using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SubscriberAPI.Application;
using SubscriberAPI.Application.RabbitQM;
using SubscriberAPI.Contracts;
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
        public async Task<IActionResult> Post([FromBody] SubscriptionRequest request)
        {
            if (request == null)
            {
                _logger.LogInformation("The subscriber is null");
                return BadRequest();
            }
            try
            {
                _messageProducer.ConsumingSubscriberMessag(request);
                var subscribtion = Subscription.CreateNewSubscription(
                    request.UserId, 
                    request.ChatId,
                    request.Name,
                    request.Brand,
                    request.Price,
                    request.Url);

                await _sudscriberService.Create(subscribtion);
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
        public async Task<IActionResult> Update([FromBody] Subscription subscriber, string userId) // переделать на subscriptionRequest
        {
            var result = await _sudscriberService.Update(userId, subscriber);// добавть  request и response  и через них проделываем маппинг к subscription
            if (result ==  false)
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
        public async Task<ActionResult<List<SubscriptionResponse>>> GetFieldsForSearch()
        {
            var listWithFields = await _sudscriberService.GetFieldsForSearchById();

            if (listWithFields == null)
            {
                return BadRequest();
            }
            var response = listWithFields.Select(s => new SubscriptionResponse
            {
                UserId = s.UserId,
                Name = s.Name,
                Brand = s.Brand,
                Price = s.Price
            });

            return Ok(response);
        }
    }


}
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SubscriberAPI.Application;
using SubscriberAPI.Application.RabbitQM;
using SubscriberAPI.Contracts;
using SubscriberAPI.Domain;
using SubscriberAPI.Infrastructure;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Xml.Linq;

namespace SubscriberAPI.Presentanion
{
    [ApiController]
    [Route("")]
    public class SubscribersController : ControllerBase
    {
        private readonly IRabbitMessageProducer _messageProducer;
        private readonly ILogger<SubscribersController> _logger;
        public readonly ISubscrieberService _sudscriberService;
        public readonly IValidator<SubscriptionRequest> _validator;

        public SubscribersController(IRabbitMessageProducer messageProducer, ILogger<SubscribersController> logger, ISubscrieberService subscrieberService, IValidator<SubscriptionRequest> validator)
        {
            _messageProducer = messageProducer;
            _logger = logger;
            _sudscriberService = subscrieberService;
            _validator = validator;
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
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }
                var subscribtion = Subscription.CreateNewSubscription
                    (
                    request.UserId, 
                    request.ChatId,
                    request.Name,
                    request.Brand,
                    request.Price,
                    request.Url
                    );

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
            var allSubscriptions = await _sudscriberService.GetAllAsync();
            if (allSubscriptions == null)
            {
                return NotFound();
            }
            var response = allSubscriptions.Select(s => new SubscriptionResponse
            {
                UserId = s.UserId,
                ChatId = s.ChatId,
                Name = s.Name,
                Brand = s.Brand,
                Price = s.Price,
                Url = s.Url
            });  
            return Ok(response);
        }

        [Route("[action]/{userId}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(string userId)
        {
            var subscription= await _sudscriberService.GetById(userId);
            if (subscription == null)
            {
                return NotFound(subscription.Error.Message); 
            }
            var response =  new SubscriptionResponse
            {
                UserId = subscription.Value.UserId,
                ChatId = subscription.Value.ChatId,
                Brand = subscription.Value.Brand,
                Name = subscription.Value.Name,
                Price = subscription.Value.Price,
                Url = subscription.Value.Url
            };
            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] SubscriptionRequest subscriptionRequest, string userId)
        {
            if (subscriptionRequest is null)
            {
                return BadRequest();
            }
            var subscription = Subscription.CreateNewSubscription
                (
                    subscriptionRequest.UserId,
                    subscriptionRequest.ChatId,
                    subscriptionRequest.Brand,
                    subscriptionRequest.Name,
                    subscriptionRequest.Price,
                    subscriptionRequest.Url
                );
            var updatedSubscription = await _sudscriberService.Update(userId, subscription);
            if (updatedSubscription ==  false)
            {
                return NotFound();
            }
            return Ok(updatedSubscription);        
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
            var ListWithfSubscriptions = await _sudscriberService.GetFieldsForSearchById();

            if (ListWithfSubscriptions == null)
            {
                return BadRequest();
            }
            var response = ListWithfSubscriptions.Select(s => new SubscriptionResponse
            {
                UserId = s.UserId,
                Name = s.Name,
                Brand = s.Brand,
                Price = s.Price
            }).ToList();
            return Ok(response);
        }
    }
    //  проверить точно ли возвращает лист с отдельными подписчиками и их продуктами
}
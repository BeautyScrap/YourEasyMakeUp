using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SubscriberAPI.Application;
using SubscriberAPI.Contracts;
using SubscriberAPI.Domain;
using SubscriberAPI.Infrastructure.RabbitQM;
using SubscriberAPI.Presentanion.Clients;

namespace SubscriberAPI.Presentanion
{
    [ApiController]
    [Route("")]
    public class SubscribersController : ControllerBase
    {
        private readonly ISubscriberRabbitMessageProducer _messageProducer;
        private readonly ILogger<SubscribersController> _logger;
        public readonly ISubscrieberService _sudscriberService;
        public readonly IValidator<SubscriptionRequest> _validator;
        public readonly IProductApiClient _productApiClient;
        public readonly ITelegramApiClient _telegramApiClient;

        public SubscribersController(ISubscriberRabbitMessageProducer messageProducer, ILogger<SubscribersController> logger, ISubscrieberService subscrieberService, IValidator<SubscriptionRequest> validator, IProductApiClient productApiClient, ITelegramApiClient telegramApiClient)
        {
            _messageProducer = messageProducer;
            _logger = logger;
            _sudscriberService = subscrieberService;
            _validator = validator;
            _productApiClient = productApiClient;
            _telegramApiClient = telegramApiClient;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] SubscriptionRequest request)
        {
            _messageProducer.ConsumingSubscriberMessag(request);
            if (request == null)
            {
                _logger.LogInformation("The subscriber is null");
                return BadRequest();
            }
            try
            {                
                var validationResult =  _validator.Validate(request);
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
                    request.Price
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
            var subscription = await _sudscriberService.GetById(userId);
            if (subscription.IsFailure)
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
                    subscriptionRequest.Price
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

        [Route("CheckPriceUpdates")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CheckPriceUpdates()
        {
            var ListWithfSubscriptions = await _sudscriberService.GetFieldsForSearchById();

            if (ListWithfSubscriptions == null)
            {
                return BadRequest();
            }
            var newProducts = await _productApiClient.GetProducts(ListWithfSubscriptions);
            if (newProducts == null) 
            { 
                return NotFound();
            }
            foreach (var product in newProducts)
            {
                await _telegramApiClient.SendFoundProduct(product);
                var userId = product.UserId;
                var name = product.Name;
                await _sudscriberService.UpdateStatusForFoundProduct(userId, name);
            }
            return Ok();
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using TelegramBotAPI.Services;
using YourEasyRent.Contracts.ProductForSubscription;
using YourEasyRent.Entities.ProductForSubscription;
using System.Text.Json;


namespace TelegramBotAPI.Controllers
{
    [ApiController]
    [Route("")]
    public class TelegramUpdateProductController : ControllerBase
    {
        private readonly ITelegramUpdateHandler _updateHandler;
        private readonly ILogger<TelegramUpdateProductController> _logger;
        public TelegramUpdateProductController(ITelegramUpdateHandler updateHandler, ILogger<TelegramUpdateProductController> logger)
        {
            _updateHandler = updateHandler;
            _logger = logger;
        }
        [HttpPut]
        [Route("UpdateProduct")]
        public async Task<IActionResult> PutProducts([FromBody] ProductForSubscriptionRequest request)
        {
            try
            {
                _logger.LogInformation("Update received: {update}", JsonSerializer.Serialize(request));
                var newProduct = ProductForSubscription.CreateFoundNewProduct
                    (
                    request.UserId,
                    request.Brand,
                    request.Name,
                    request.Price
                    );
                newProduct.SetUrlAndUrlImage(request.Url, request.ImageUrl);
                await _updateHandler.HandlerUpdateAsync(newProduct);
                // AK TODO тут еще проверку сделать какой ответ пришел или booll
                return Ok();
            }
            catch(Exception ex)
            { 
                return BadRequest(ex.Message);
            }
        }

    }
}

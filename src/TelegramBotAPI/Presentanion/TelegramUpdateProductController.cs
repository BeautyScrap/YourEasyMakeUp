using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TelegramBotAPI.Services;
using YourEasyRent.Contracts.ProductForSubscription;
using YourEasyRent.Entities.ProductForSubscription;

namespace TelegramBotAPI.Controllers
{
    [ApiController]
    [Route("")]
    public class TelegramUpdateProductController : ControllerBase
    {
        private readonly ITelegramUpdateHandler _updateHandler;
        public TelegramUpdateProductController(ITelegramUpdateHandler updateHandler)
        {
            _updateHandler = updateHandler;
        }
        [HttpPut]
        [Route("UpdateProduct")]
        public async Task<IActionResult> PutProducts([FromBody] ProductForSubscriptionRequest request)
        {
            try
            {
                var newProduct = ProductForSubscription.CreateFoundNewProduct
                    (
                    request.UserId,
                    request.Brand,
                    request.Name,
                    request.Price
                    );
                newProduct.SetUrlAndUrlImage(request.Url, request.UrlImage);
                await _updateHandler.HandlerUpdateAsync(newProduct);
                // AK TODO тут еще проверку сделать какой ответ пришел или bool
                return Ok();
            }
            catch(Exception ex )
            { 
                return BadRequest(ex.Message);
            }
        }

    }
}

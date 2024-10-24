using Microsoft.AspNetCore.Mvc;
using YourEasyRent.Services;
using YourEasyRent.DataBase.Interfaces;
using Telegram.Bot.Types;
using YourEasyRent.Entities.ProductForSubscription;
using YourEasyRent.Contracts.ProductForSubscription;

namespace YourEasyRent.Controllers;

[ApiController]
[Route("")]
public class TelegramCallbackController : ControllerBase
{
    private readonly ITelegramCallbackHandler _handler;
    private readonly ILogger<TelegramCallbackController> _logger;
  
    public TelegramCallbackController(ITelegramCallbackHandler handler, ILogger<TelegramCallbackController> logger)
    {
        _handler = handler;
        _logger = logger;
    }

    [HttpPost] 
    [Route("telegram/callback")]  
    public async Task<IActionResult> ProcessCallback([FromBody] Update update)
    {
        try
        {
            TgButtonCallback tgButtonCallback = new TgButtonCallback(update);
            await _handler.HandleUpdateAsync(tgButtonCallback);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "[ProcessCallback] : Callback is not correct");
            return BadRequest(ex);
        }
        _logger.LogInformation("CallbackIsDone");
        return Ok();
    }

    [HttpPost]
    [Route("telegram/subscription")]
    public async Task<IActionResult> SendProductAnswer([FromBody] ProductForSubscriptionRequest request)// AK TODO  стоит ли сделать другой кла
    {
        try
        {
            if (request == null)
            { 
                return BadRequest(); 
            }
            ProductForSubscription product = ProductForSubscription.CreateProductForSearch
                (
                request.UserId,
                request.ChatId,
                request.Brand,
                request.Name,
                request.Price,
                request.Url
                );
            // AK TODO  тут какой то хендлер/ Сервис должен быть , чтобы передать в него объект product  и внутри сервиса по UserId передать найденный продукт  пользаку
        }// будет ли корректно сделать еще один хендлер для отправки сообщения?наверно да, потому что мы сами отправляем пользаку ответ, а не ждем его коллбека как в TelegramCallbackHandler сервисе
        catch (Exception ex)
        {
            _logger.LogError(ex, "[SendProductAnswer] : Product answer is not correct");
            return BadRequest(ex);
        }
        _logger.LogInformation("SendProductAnswerIsDone");
        return Ok();
    }

}


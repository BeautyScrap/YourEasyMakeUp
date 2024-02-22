using Microsoft.AspNetCore.Mvc;
using YourEasyRent.Services;
using YourEasyRent.DataBase.Interfaces;
using Telegram.Bot.Types;

namespace YourEasyRent.Controllers;

[ApiController]
[Route("")] // It should not have any prefix
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
            await _handler.HandleUpdateAsync(update);
        }
        catch (Exception e) 
        {
            var test = e;
            // we swallow all exception so tg recives OK and does not resend an Update
        }
        _logger.LogInformation("CallbackIsDone");
        return Ok();
    } 
}


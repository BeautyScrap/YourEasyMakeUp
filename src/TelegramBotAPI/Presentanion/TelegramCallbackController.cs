using Microsoft.AspNetCore.Mvc;
using YourEasyRent.Services;
using Telegram.Bot.Types;
using System.Text.Json;




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
            _logger.LogInformation("Update received: {update}", JsonSerializer.Serialize(update));

            TgButtonCallback tgButtonCallback = new TgButtonCallback(update);
            await _handler.HandleCallbackAsync(tgButtonCallback);
        }
        catch (Exception ex) 
        { 
            _logger.LogError( "[ProcessCallback] : Callback is not correct",ex);
            return BadRequest(ex);
        }
        _logger.LogInformation("CallbackIsDone");
        return Ok();
    }
   
}


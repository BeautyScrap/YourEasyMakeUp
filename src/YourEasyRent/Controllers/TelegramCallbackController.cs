using Microsoft.AspNetCore.Mvc;
using YourEasyRent.Services;
using YourEasyRent.DataBase.Interfaces;
using Telegram.Bot.Types;

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
            tgButtonCallback.IsBotStart();
            tgButtonCallback.GetUserId(update);
            tgButtonCallback.GetChatId(update);
            tgButtonCallback.IsValidMsg();
            tgButtonCallback.GetNameOfButton(update);
            tgButtonCallback.IsMenuButton();
            tgButtonCallback.IsProductButton();

            await _handler.HandleUpdateAsync(tgButtonCallback);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "[ProcessCallback] : Callback is not correct");
            var test = ex; 
        }
        _logger.LogInformation("CallbackIsDone");
        return Ok();
    } 
}


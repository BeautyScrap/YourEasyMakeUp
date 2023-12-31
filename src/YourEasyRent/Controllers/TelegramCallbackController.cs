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

    public TelegramCallbackController(ITelegramCallbackHandler handler)
    {
        _handler = handler;
    }

    [HttpPost] 
    [Route("telegram/callback")]  
    public async Task<IActionResult> ProcessCallback(Update update, CancellationToken token)
    {
        try
        {
            await _handler.HandleUpdateAsync(update, token);
        }
        catch (Exception e) 
        {
            // we swallow all exception so tg recives OK and does not resend an Update
        }

        return Ok();
    } 
}


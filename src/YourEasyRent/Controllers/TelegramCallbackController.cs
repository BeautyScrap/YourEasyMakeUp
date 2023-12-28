using Microsoft.AspNetCore.Mvc;
using YourEasyRent.Services;
using YourEasyRent.DataBase.Interfaces;

namespace YourEasyRent.Controllers;

[ApiController]
[Route("")] // It should not have any prefix
public class TelegramCallbackController : ControllerBase
{
    private readonly IProductRepository _repository; 

    public TelegramCallbackController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpPost] 
    [Route("telegram/callback")]  
    public async Task<IActionResult> ProcessCallback()
    {
        return Ok();
    } 
}


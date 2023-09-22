using Microsoft.AspNetCore.Mvc;
using YourEasyRent.Services;

namespace YourEasyRent.Controllers
{
    public class TelegramBotController : ControllerBase
    {
        private readonly TelegramBotServices _botService;

        public TelegramBotController(string botToken)
        {
            _botService = new TelegramBotServices(botToken);
        }

        public void Start()
        {
            _botService.StartReceivingMessages();   
        }
    }

}

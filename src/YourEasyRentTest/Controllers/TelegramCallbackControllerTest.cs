using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using YourEasyRent.Controllers;
using YourEasyRent.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace YourEasyRentTest.Controllers
{
    public class TelegramCallbackControllerTest
    {


        private readonly TelegramCallbackController _controller;
        private readonly Mock<ILogger<TelegramCallbackController>> _mockLogger; 
        private readonly Mock<ITelegramCallbackHandler> _mockHandler;
        public TelegramCallbackControllerTest()
        {
            _mockHandler = new Mock<ITelegramCallbackHandler>();
            _mockLogger = new Mock<ILogger<TelegramCallbackController>>();
            _controller = new TelegramCallbackController(_mockHandler.Object, _mockLogger.Object);
        }
        [Fact]
        public async Task ProcessCallback_ReturnOK()
        {
            //Arrange
            var update = new Update();

            //Act
            var result= await _controller.ProcessCallback(update);  

            //Assert
            result.Should().BeOfType<OkResult>();
        }


        [Fact]
        public async Task ProcessCallback_ReturnBadRequest()
        {
            //Arrange
            var update =  new Update();
            var exception = new Exception("Test Exception");
            _mockHandler.Setup(h => h.HandleUpdateAsync(It.IsAny<TgButtonCallback>())).ThrowsAsync(exception);
           
            //Act
            var result = await _controller.ProcessCallback(update);
            //Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be(exception);
        }

    }
}

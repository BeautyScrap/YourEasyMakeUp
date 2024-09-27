using DnsClient.Internal;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourEasyRent.Controllers;
using YourEasyRent.Entities;
using YourEasyRent.Services;

namespace YourEasyRentTest.Controllers
{
    public class SubscribersControllerTest
    {
        private readonly SubscribersController _controller;
        private readonly Mock<ILogger<SubscribersController>> _mockLogger;
        private readonly Mock<IRabbitMessageProducer> _mockRabbit;

        public SubscribersControllerTest()
        {
            _mockRabbit = new Mock<IRabbitMessageProducer>();
            _mockLogger = new Mock<ILogger<SubscribersController>>();
            _controller = new SubscribersController(_mockRabbit.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task SendSubscriber_ReturnOk()
        {
            //Arrange
            var subscriber = new ProductForSubscription();
            _controller.ModelState.Clear();

            //Act
            var result = _controller.SendSubscriber(subscriber);

            //Assert
            result.Should().BeOfType<OkResult>();

        }

        [Fact]
        public async Task SendSubscriber_IsNotValid_ReturnBadRequest()
        {
            //Arange
            var subscriber = new ProductForSubscription();
            _controller.ModelState.AddModelError("Error", "ModelState is invalid");
            //Act
            var result = _controller.SendSubscriber(subscriber);

            //Assert
            result.Should().BeOfType<BadRequestResult>();

        }
    }
}

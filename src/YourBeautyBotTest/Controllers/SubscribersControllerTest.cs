using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Validations;
using Moq;
using SubscriberAPI.Application;
using SubscriberAPI.Application.RabbitQM;
using SubscriberAPI.Contracts;
using SubscriberAPI.Domain;
using SubscriberAPI.Infrastructure.Clients;
using SubscriberAPI.Presentanion;
using System.Security.Cryptography.Xml;

namespace YourBeautyBotTest.Controllers
{
    public class SubscribersControllerTest
    {
        private readonly Mock<ISubscriberRabbitMessageProducer> _mockRabbit;
        private readonly Mock<ILogger<SubscribersController>> _mockLogger;
        private readonly Mock<ISubscrieberService> _mockService;
        private readonly Mock<IValidator<SubscriptionRequest>> _mockValidator;
        private readonly Mock<IProductApiClient> _mockPdClient;
        private readonly Mock<ITelegramApiClient> _mockTgClient;
        private readonly SubscribersController _subscribersController;

        public SubscribersControllerTest() 
        {
            _mockRabbit = new Mock<ISubscriberRabbitMessageProducer>();
            _mockLogger = new Mock<ILogger<SubscribersController>>();
            _mockService = new Mock<ISubscrieberService>(); 
            _mockValidator = new Mock<IValidator<SubscriptionRequest>>();
            _mockPdClient = new Mock<IProductApiClient>();  
            _mockTgClient = new Mock<ITelegramApiClient>();
            _subscribersController = new SubscribersController
                (
                _mockRabbit.Object,
                _mockLogger.Object,
                _mockService.Object,
                _mockValidator.Object,
                _mockPdClient.Object,
                _mockTgClient.Object
                );
        }

        [Fact]
        public async Task Post_ShouldReturnOk_WhenRequestIsValid()
        {
            //Arrange
            var request = new SubscriptionRequest 
            { 
                UserId = "123",
                Brand = "BrandTest",
                Name = "NameTest",
                Price = 100,
                Url = "UrlTest"
            };
            _mockValidator.
                Setup(v => v.Validate(request)).
                Returns(new ValidationResult());

            _mockService.Setup(sub => sub.Create(It.IsAny<Subscription>())).
                Returns(Task.CompletedTask); 
            
            //Act
            var result = await _subscribersController.Post(request);

            //Assert
            var okResult =  Assert.IsType<OkResult>(result);
            _mockRabbit.Verify(r => r.ConsumingSubscriberMessag(request), Times.Once());
            _mockService.Verify(s => s.Create(It.IsAny<Subscription>()), Times.Once());
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequest_WhenValidationFails()
        {
            var request = new SubscriptionRequest();
            _mockValidator.
                Setup(v => v.Validate(request)).
                Returns(new ValidationResult
                {
                    Errors = new List<ValidationFailure> { new ValidationFailure("Name", "Name is required") }
                });
            // Act
            var result = await _subscribersController.Post(request);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<List<ValidationFailure>>(badRequestObjectResult.Value);
            Assert.Single(errors);
            Assert.Equal("Name is required", errors[0].ErrorMessage);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequest_WhenRequestIsNull() 
        {
            // Act
            var result = await _subscribersController.Post(null);

            // Assert
            Assert.Contains(_mockLogger.Invocations, invocation =>
                   invocation.Method.Name == nameof(ILogger.Log) &&
                   invocation.Arguments[0].ToString() == "Information" &&
                   invocation.Arguments[2].ToString() == "The subscriber is null");
        }
    }
}

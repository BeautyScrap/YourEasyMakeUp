//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using YourEasyRent.Controllers;
//using Microsoft.Extensions.Logging;
//using YourEasyRent.Services;
//using YourEasyRent.Contracts.ProductForSubscription;
//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc;
//using YourEasyRent.Entities.ProductForSubscription;
//using Microsoft.AspNetCore.Http.HttpResults;

//namespace YourEasyRentTest.Controllers
//{
//    public class SubscriptionControllerTest
//    {
//        private readonly SubscriptionController _subscriptionController;
//        private readonly Mock<ILogger<SubscriptionController>> _mockLogger;
//        private readonly Mock<IRabbitMessageProducer> _rabbitMessageProducer;
//        private readonly Mock<IProductForSubscriptionService> _service;

//        public SubscriptionControllerTest()
//        { 
//            _mockLogger = new Mock<ILogger<SubscriptionController>>();
//            _rabbitMessageProducer = new Mock<IRabbitMessageProducer>();
//            _service = new Mock<IProductForSubscriptionService>();
//            _subscriptionController = new SubscriptionController(_rabbitMessageProducer.Object,_service.Object, _mockLogger.Object);       
//        }

//        [Fact]
//        public async Task Search_ReturnOK_WhenSubscriptionProductRequestIsValid()
//        {
//            //Arrange
//            var productRequest = new List<ProductForSubscriptionRequest>()
//            {
//                new SubscribersProductRequest("1","1", "TestBrand1", "TestName1", 10),
//                new SubscribersProductRequest("2","2", "TestBrand2", "TestName2", 20)
//            };

//            //Act
//            var result = await _subscriptionController.Search(productRequest);

//            //Assert
//            result.Should().BeOfType<OkResult>();
//            _rabbitMessageProducer.Verify(rab => rab.ConsumingSubscriberMessage(productRequest),Times.Once());
//            _service.Verify(s => s.ProductHandler(It.IsAny<List<ProductForSubscription>>()),Times.Once());
//        }

//        [Fact]
//        public async Task Search_ReturnBadRequest_WhenSubscriptionProductRequestIsNull()
//        {
//            //Act
//            var result = await _subscriptionController.Search(null);

//            //Assert
//            result.Should().BeOfType<BadRequestResult>();
//        }

//        [Fact]
//        public async Task Search_ReturnsInternalServerError_OnException()
//        {
//            //Arrange
//            var productRequest = new List<ProductForSubscriptionRequest>()
//            {
//                new SubscribersProductRequest("1","1", "TestBrand1", "TestName1", 10),
//                new SubscribersProductRequest("2","2", "TestBrand2", "TestName2", 20)
//            };
//            _service.Setup(s => s.ProductHandler(It.IsAny<List<ProductForSubscription>>())).Throws(new System.Exception("Test exception"));

//            //Act
//            var result = await _subscriptionController.Search(productRequest);

//            //Assert
//            var statusCodeResult = result as ObjectResult;
//            statusCodeResult.Should().NotBeNull();
//            statusCodeResult.StatusCode.Should().Be(500);
//            statusCodeResult.Value.Should().Be("Failed to check the SubscribersProductRequest");

//        }

//    }
//}

using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotAPI.Controllers;
using TelegramBotAPI.Services;
using YourEasyRent.Contracts.ProductForSubscription;
using YourEasyRent.Entities.ProductForSubscription;

namespace YourEasyRentTest.Controllers
{
    public class TelegramUpdateProductControllerTest
    {
        private readonly TelegramUpdateProductController _controller;
        private readonly Mock<ITelegramUpdateHandler> _mockHandler;

        public TelegramUpdateProductControllerTest()
        {
            _mockHandler = new Mock<ITelegramUpdateHandler>();
            _controller = new TelegramUpdateProductController(_mockHandler.Object); 
        }
        [Fact]
        public async Task PutProduct_ReturnOkResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            var request = new ProductForSubscriptionRequest
                (
                "user123",          
                "BrandX",          
                "ProductName",       
                99.99m,               
                "http://example.com", 
                "http://example.com/image.jpg" 
                );

            //Act
            var result = await _controller.PutProducts(request);

            //Assert
            result.Should().BeOfType<OkResult>();
            _mockHandler.Verify(handler => handler.HandlerUpdateAsync(It.Is<ProductForSubscription>(
            p => p.UserId == request.UserId &&
                 p.Brand == request.Brand &&
                 p.Name == request.Name &&
                 p.Price == request.Price &&
                 p.Url == request.Url &&
                 p.UrlImage == request.UrlImage))
            ,Times.Once);
        }

        [Fact]
        public async Task PutProduct_ReturnBadRequest_WhenExceptionIsThrown()
        {
            //Arrange
            var request = new ProductForSubscriptionRequest
               (
               "user123",            
               "BrandX",             
               "ProductName",        
               99.99m,           
               "http://example.com", 
               "http://example.com/image.jpg"
               );
            _mockHandler.Setup(handler => handler.HandlerUpdateAsync(It.IsAny<ProductForSubscription>())).ThrowsAsync(new Exception("Exception"));
            // Act
            var result = await _controller.PutProducts(request);
            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult!.Value.Should().Be("Exception"); 
        }
    }
}

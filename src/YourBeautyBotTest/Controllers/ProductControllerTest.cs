using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProductAPI.Application;
using ProductAPI.Application.RabbitMQ;
using ProductAPI.Contracts.TelegramContract;
using ProductAPI.Controllers;
using ProductAPI.Domain.ProductForUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace YourBeautyBotTest.Controllers
{
    public class ProductControllerTest
    {
        private readonly Mock<IRabbitMessageProducer> _mockRabbit;
        private readonly Mock<ILogger<ProductController>> _mockLogger;
        private readonly Mock<IProductForSubService> _mockServiceForSub;
        private readonly Mock<IProductForUserService> _mockServiceForUser;

        private readonly ProductController _controller;

        public ProductControllerTest() 
        {
            _mockRabbit = new Mock<IRabbitMessageProducer>();
            _mockLogger = new Mock<ILogger<ProductController>>();
            _mockServiceForSub = new Mock<IProductForSubService>(); 
            _mockServiceForUser = new Mock<IProductForUserService>();
            _controller = new ProductController
                (
                _mockLogger.Object,
                _mockRabbit.Object,
                _mockServiceForSub.Object,
                _mockServiceForUser.Object
                );       
        }
        [Fact]
        public async Task SearchOneProductForUser_ReturnOk_WhenFoundProduct()
        {
            //Arrange
            var request = new SearchProductResultRequest("TestBrand", "TestCategory");
            var searchProducts = ProductResultForUser.CreateProductForSearch("TestBrand", "TestCategory");
            var dto = new AvaliableResultForUserDto
            {
                Brand = "TestBrand",
                Name = "TestProduct",
                Category = "TestCategory",
                Price = 100,
                Url = "http://example.com/product",
                ImageUrl = "http://example.com/image.jpg"
            };

            var foundProduct = AvaliableResultForUser.FromDto(dto);

            _mockServiceForUser
                    .Setup(s => s.HandlerOne(It.IsAny<ProductResultForUser>()))
                    .ReturnsAsync(foundProduct);

            //Act
            var result = await _controller.SearchOneProductForUser(request);

            //Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            //okResult!.Value.Should().BeOfType<FoundProductResultResponse>();


            var response = okResult!.Value as FoundProductResultResponse;
            response.Should().NotBeNull();
            response!.Brand.Should().Be("TestBrand");
            response.Category.Should().Be("TestCategory");
            response.Name.Should().Be("TestProduct");
            response.Price.Should().Be(100);
            response.Url.Should().Be("http://example.com/product");
            response.ImageUrl.Should().Be("http://example.com/image.jpg");

        }
    }

}

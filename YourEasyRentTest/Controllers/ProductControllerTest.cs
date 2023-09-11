using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using FluentAssertions;
using YourEasyRent.Entities;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Controllers;
using YourEasyRent.Services;
using Microsoft.AspNetCore.Mvc;

namespace YourEasyRentTest.Controllers
{
    public class ProductControllerTest
    {
        [Fact]
        public async Task GetProducts_Return_AllProducts()
        {
            // arrange
            var products = new List<Product>{new Product { Id = "test", Name = "Product 1" }, new Product { Id = "test", Name = "Product 2" } };       
            var productRepoMock = new Mock<IProductRepository>(); 
            productRepoMock.Setup(repo => repo.GetProducts()).ReturnsAsync(products);
            var controller = new ProductController(productRepoMock.Object); // Создала экземпляр контроллера ProductsController, передавая фейковый repositoryMock в конструктор.

            // act
            var result = await controller.GetProducts();

            // assert
            result.Should().BeOfType<ActionResult<IEnumerable<Product>>>()
                .Which.Value.Should().BeAssignableTo<IEnumerable<Product>>()
                .And.HaveCount(products.Count); // проверряем является ли результат объектом типа <ActionResult<IEnumerable<Product>, в котором значение Value является IEnumerable<Product> и проверяем что количество элементов в IEnumerable<Product> равно количеству продуктов, которые мы заранее подготовили.

        }

        [Fact]
        public async Task GetProducts_Return_ProductsNotFound()
        {
            //arrange
            var productRepoMock =new Mock<IProductRepository>();
            productRepoMock.Setup<Task<IEnumerable<Product>?>>(repo => repo.GetProducts()).ReturnsAsync((IEnumerable<Product>?)null);
            var controller = new ProductController(productRepoMock.Object);
            //act
            var result = await controller.GetProducts();

            //assert
            result.Should().BeOfType<ActionResult<IEnumerable<Product>?>>() // явно указываем ActionResult<IEnumerable<Product>?> с использованием аннотации nullability '?', и проверяем, что внутренний результат (.Result) также является NotFoundResult.
                .Which.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetPriducts_Return_Exception()
        {
            // arrange
            var productRepoMock = new Mock<IProductRepository>();
            productRepoMock.Setup(repo => repo.GetProducts()).ThrowsAsync(new Exception("Test exseption"));
            var controller = new ProductController(productRepoMock.Object);

            //act
            var result = await controller.GetProducts();

            //assert
            result.Should().BeOfType<ActionResult<IEnumerable<Product>>>()// использую ActionResult<IEnumerable<Product>?> и затем проверяем, что внутренний результат (.Result) является ObjectResult, а затем проверяем статус кода 500.
                .Which.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);           

        }

        [Fact]
        public async Task GetProductById_Return_OkResult()
        {
            //arrange
            string OkProductId = "OkProductId";
            var productRepoMock = new Mock<IProductRepository>();
            productRepoMock.Setup(repo => repo.Get(It.IsAny<string>())).ReturnsAsync(new Product { Id = OkProductId });
            var controller = new ProductController(productRepoMock.Object);

            //act
            var result = await controller.Get(OkProductId);

            //assert
            result.Should().BeOfType<ActionResult<Product>>()
                .Which.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<Product>();

        }
        [Fact]
        public async Task GetProductById_Return_NotFound()
        {
            //arrange
            string NotProductId = "NotProductId";
            var productRepoMock = new Mock<IProductRepository>();
            productRepoMock.Setup(repo =>repo.Get(It.IsAny<string>())).ReturnsAsync((Product?)null);
            var controller = new ProductController(productRepoMock.Object);

            //act
            var result = await controller.Get(NotProductId);


            //assert
            result.Should().BeOfType<ActionResult<Product>>()
                .Which.Result.Should().BeOfType<NotFoundResult>();
                //.Which.Value.Should().BeOfType<Product>();
        }
            





    }


}

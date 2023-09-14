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
using YourEasyRent.Entities.Douglas;

namespace YourEasyRentTest.Controllers
{
    public class ProductControllerTest
    {
        [Fact]
        public async Task GetProducts_Return_AllProducts()
        {
            // arrange
            var products = new List<Product> { new Product { Id = "test", Name = "Product 1" }, new Product { Id = "test", Name = "Product 2" } };
            var productRepoMock = new Mock<IProductRepository>();
            productRepoMock.Setup(repo => repo.GetProducts()).ReturnsAsync(products);
            var controller = new ProductController(productRepoMock.Object); // Создала экземпляр контроллера ProductsController, передавая фейковый repositoryMock в конструктор.

            // act
            var result = await controller.GetProducts();

            // assert
            result.Should().BeOfType<ActionResult<IEnumerable<Product>>>()
                .Which.Value.Should().BeAssignableTo<IEnumerable<Product>>()// проверряем является ли результат объектом типа <ActionResult<IEnumerable<Product>, в котором значение Value является IEnumerable<Product> и проверяем что количество элементов в IEnumerable<Product> равно количеству продуктов, которые мы заранее подготовили.
                .Which.Should().HaveCount(2)
                .And.Subject.Should().BeEquivalentTo(products);         // проверяем , что поля поля «id» или «Name», отображаются правильно.



        }

        //[Fact]
        //public async Task GetProducts_Return_ProductsNotFound()
        //{
        //    //arrange
        //    var productRepoMock =new Mock<IProductRepository>();
        //    productRepoMock.Setup<Task<IEnumerable<Product>?>>(repo => repo.GetProducts()).ReturnsAsync((IEnumerable<Product>?)null);
        //    var controller = new ProductController(productRepoMock.Object);
        //    //act
        //    var result = await controller.GetProducts();

        //    //assert
        //    result.Should().BeOfType<ActionResult<IEnumerable<Product>?>>() // явно указываем ActionResult<IEnumerable<Product>?> с использованием аннотации nullability '?', и проверяем, что внутренний результат (.Result) также является NotFoundResult.
        //        .Which.Result.Should().BeOfType<NotFoundResult>();
        //}

        [Fact]
        public async Task GetProducts_WhenRepositoryThrowsException_ShouldReturn500()
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
            productRepoMock.Setup(repo => repo.Get(It.IsAny<string>())).ReturnsAsync((Product?)null);
            var controller = new ProductController(productRepoMock.Object);

            //act
            var result = await controller.Get(NotProductId);


            //assert
            result.Should().BeOfType<ActionResult<Product>>()
                .Which.Result.Should().BeOfType<NotFoundResult>();

        }
        [Fact]
        public async Task GetProductById_Return_Exception()
        {
            //arrange
            string ExceptionId = "ExceptionId";
            var productRepoMock = new Mock<IProductRepository>();
            productRepoMock.Setup(repo => repo.Get(It.IsAny<string>())).ThrowsAsync(new Exception("ExceptionId"));
            var controller = new ProductController(productRepoMock.Object);

            //act
            var result = await controller.Get(ExceptionId);
            //assert
            result.Should().BeOfType<ActionResult<Product>>()
                .Which.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);
        }


        [Fact]
        public async Task GetProductByBrand_Returns_OkResult()
        {
            //arrange
            string brandWithOkResult = "brandWithOkResult";
            var products = new List<Product> { new Product { Id = "test", Brand = "BrandProduct" }, new Product { Id = "test", Brand = "BrandProduct" } };
            var productRepoMock = new Mock<IProductRepository>();
            productRepoMock.Setup(repo => repo.GetByBrand(brandWithOkResult)).ReturnsAsync(products);
            var controller = new ProductController(productRepoMock.Object); // Создала экземпляр контроллера ProductsController, передавая фейковый repositoryMock в конструктор.

            // act
            var result = await controller.GetProductByBrand(brandWithOkResult);

            // assert
            result.Should().BeOfType<ActionResult<IEnumerable<Product>>>()
                .Which.Result.Should().BeOfType<OkObjectResult>();

        }

        [Fact]
        public async Task GetProductByBrand_Returns_NotFound()
        {
            //arrange
            string brandNotFound = "branbNotFound";
            var productRepoMock = new Mock<IProductRepository>();
            productRepoMock.Setup(repo => repo.GetByBrand(brandNotFound)).ReturnsAsync((IEnumerable<Product>?)null);
            var controller = new ProductController(productRepoMock.Object);

            // act
            var result = await controller.GetProductByBrand(brandNotFound);

            //assert
            result.Should().BeOfType<ActionResult<IEnumerable<Product>>>()
                .Which.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetProductByBrand_Return_Exception()
        {
            //arrange
            string ExceptionBrand = "ExceptionBrand";
            var productRepoMock = new Mock<IProductRepository>();
            productRepoMock.Setup(repo => repo.Get(It.IsAny<string>())).ThrowsAsync(new Exception("ExceptionBrand"));
            var controller = new ProductController(productRepoMock.Object);

            //act
            var result = await controller.Get(ExceptionBrand);
            //assert
            result.Should().BeOfType<ActionResult<Product>>()
                .Which.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task GetProductByName_Return_OkResult()
        {
            //arrange
            string okProductName = "OkProductName";
            var productRepoMock = new Mock<IProductRepository>();
            productRepoMock.Setup(repo => repo.Get(It.IsAny<string>())).ReturnsAsync(new Product { Name = okProductName });
            var controller = new ProductController(productRepoMock.Object);

            //act
            var result = await controller.Get(okProductName);

            //assert
            result.Should().BeOfType<ActionResult<Product>>()
                .Which.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetProductByName_Return_NotFound()
        {
            //arrange
            string productNameNotFound = "ProductNameNotFound";
            var productRepoMock = new Mock<IProductRepository>();
            var controller = new ProductController(productRepoMock.Object);

            //act
            var result = await controller.Get(productNameNotFound);

            //assert
            result.Should().BeOfType<ActionResult<Product>>()
                .Which.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetProductByName_Return_Exception()
        {
            //arrange
            string ExceptionName = "ExceptionName";
            var productRepoMock = new Mock<IProductRepository>();
            productRepoMock.Setup(repo => repo.Get(It.IsAny<string>())).ThrowsAsync(new Exception("ExceptionName"));
            var controller = new ProductController(productRepoMock.Object);

            //act
            var result = await controller.Get(ExceptionName);
            //assert
            result.Should().BeOfType<ActionResult<Product>>()
                .Which.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task Post_CreateNewProduct_Successfully()
        {
            // Arrange
            var product = new Product();
            var productRepoMock = new Mock<IProductRepository>();
            productRepoMock.Setup(repo => repo.Create(product)).Returns(Task.CompletedTask);
            var controller = new ProductController(productRepoMock.Object);

            // act
            var result = await controller.Post(product) as CreatedAtActionResult; // приведение результата выполнения метода Post к ожидаемым типам CreatedAtActionResult

            //assert
            result.Should().NotBeNull();
            result.ActionName.Should().Be(nameof(ProductController.Get)); //  проверяем имя действия
            result.Value.Should().BeEquivalentTo(product);

        }

        [Fact]
        public async Task Post_CreateNewProduct_Extencion()
        {
            //arrange
            var product = new Product();
            var productRepoMock = new Mock<IProductRepository>();
            productRepoMock.Setup(repo => repo.Create(product)).ThrowsAsync(new Exception("Test failed to create user"));
            var controller = new ProductController(productRepoMock.Object);
            //act
            var result = await controller.Post(product);

            //assert
            result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);

        }

        [Fact]
        public async Task UpdateProduct_WhenProductExsist_RsturnsOkResult()
        {
            //arrange
            var exisistinId = "exisistinId";
            var updateProduct = new Product { Id = exisistinId, Name = "UpdateProductName" };
            var productRepoMock = new Mock<IProductRepository>();
            productRepoMock.Setup(repo => repo.Get(exisistinId)).ReturnsAsync(new Product { Id = exisistinId, Name = "UpdateProductName" });
            var controller = new ProductController(productRepoMock.Object);
            //act
            var result = await controller.UpdateProduct(exisistinId, updateProduct);

            //assert
            //result.Should().BeOfType<ActionResult<Product>>()
            result.Should().BeOfType<OkResult>();
            productRepoMock.Verify(repo => repo.Update(exisistinId, updateProduct), Times.Once);//  проверяем был ли вызван метод Update для объекта productMockRepository с аргументами exisistinId один раз)


        }

        [Fact]
        public async Task UpdateProduct_WhenProductNotExsist_ReturnNotFoundResult()

        {
            //arrange
            var notExisistinId = "notExisistinId";
            var updateProduct = new Product { Id = notExisistinId, Name = "NotUpdateProductName" };
            var productRepoMock = new Mock<IProductRepository>();
            productRepoMock.Setup(repo => repo.Get(notExisistinId)).ReturnsAsync((Product?)null);
            var controller = new ProductController(productRepoMock.Object);

            //act
            var result = await controller.UpdateProduct(notExisistinId,updateProduct);

            //assert
            
            result.Should().BeOfType<NotFoundResult>();       
        }

        [Fact]  
        public async Task UpdateProduct_WhenExeption_Return500StatusCode()
        {
            //arrange
            var exeptionId = "exceptionId";
            var updateProduct = new Product { Id = exeptionId, Name = "Exception" };
            var productRepoMock = new Mock<IProductRepository>();
            productRepoMock.Setup(repo =>repo.Get(exeptionId)).ThrowsAsync(new Exception());
            var controller = new ProductController(productRepoMock.Object);
            
            //act
            var result = await controller.UpdateProduct(exeptionId,updateProduct);
            //assert

            result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);
            


        }

    }


   }

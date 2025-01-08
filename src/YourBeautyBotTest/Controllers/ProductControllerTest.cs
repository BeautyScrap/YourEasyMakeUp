//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Moq;
//using FluentAssertions;
//using YourEasyRent.Entities;
//using YourEasyRent.DataBase.Interfaces;
//using YourEasyRent.Controllers;
//using YourEasyRent.Services;
//using Microsoft.AspNetCore.Mvc;
//using YourEasyRent.Entities.Douglas;
//using Microsoft.AspNetCore.Mvc.Infrastructure;
//using Microsoft.Extensions.Logging;

//namespace YourEasyRentTest.Controllers
//{
//    public class ProductControllerTest
//    {
//        [Fact]
//        public async Task GetProducts_WhenAllProductsFound_ReturnOkResult()
//        {
//            // arrange
//            var products = new List<Product> { new Product { Id = "test", Name = "Product 1" }, new Product { Id = "test", Name = "Product 2" } };
//            var productRepoMock = new Mock<IProductRepository>();
//            var loggerMock = new Mock<ILogger<ProductController>>();

//            productRepoMock.Setup(repo => repo.GetProducts()).ReturnsAsync(products);
//            var controller = new ProductController(productRepoMock.Object, loggerMock.Object, _rabbitMessageProducer.Object, _service.Object); // Создала экземпляр контроллера ProductsController, передавая фейковый repositoryMock в конструктор.

//            // act
//            var result = await controller.GetProducts();

//            // assert
//            result.Should().BeOfType<ActionResult<IEnumerable<Product>>>()
//                .Which.Value.Should().BeAssignableTo<IEnumerable<Product>>()// проверряем является ли результат объектом типа <ActionResult<IEnumerable<Product>, в котором значение Value является IEnumerable<Product> и проверяем что количество элементов в IEnumerable<Product> равно количеству продуктов, которые мы заранее подготовили.
//                .Which.Should().HaveCount(2)
//                .And.Subject.Should().BeEquivalentTo(products);         // проверяем , что поля поля «id» или «Name», отображаются правильно.
//        }

//        [Fact]
//        public async Task GetProducts_WhenProductsNotFound_ReturnNotFoundResult()
//        {
//            //arrange
//            var productRepoMock = new Mock<IProductRepository>();
//            var loggerMock = new Mock<ILogger<ProductController>>();
//            productRepoMock.Setup<Task<IEnumerable<Product>?>>(repo => repo.GetProducts()).ReturnsAsync((IEnumerable<Product>?)null);
//            var controller = new ProductController(productRepoMock.Object, loggerMock.Object);
//            //act
//            var result = await controller.GetProducts();

//            //assert
//            result.Should().BeOfType<ActionResult<IEnumerable<Product>?>>() // явно указываем ActionResult<IEnumerable<Product>?> с использованием аннотации nullability '?', и проверяем, что внутренний результат (.Result) также является NotFoundResult.
//                .Which.Result.Should().BeOfType<NotFoundResult>();
//        }

//        [Fact]
//        public async Task GetProducts_WhenRepositoryThrowsException_ShouldReturn500()
//        {
//            // arrange
//            var productRepoMock = new Mock<IProductRepository>();
//            var loggerMock = new Mock<ILogger<ProductController>>();
//            productRepoMock.Setup(repo => repo.GetProducts()).ThrowsAsync(new Exception("Test exseption"));
//            var controller = new ProductController(productRepoMock.Object, loggerMock.Object);

//            //act
//            var result = await controller.GetProducts();

//            //assert
//            result.Should().BeOfType<ActionResult<IEnumerable<Product>>>()// использую ActionResult<IEnumerable<Product>?> и затем проверяем, что внутренний результат (.Result) является ObjectResult, а затем проверяем статус кода 500.
//                .Which.Result.Should().BeOfType<ObjectResult>()
//                .Which.StatusCode.Should().Be(500);
//        }

//        [Fact]
//        public async Task GetProductById_WhenProductFoundById_ReturnOkResult()
//        {
//            //arrange
//            var OkProductId = "OkProductId";
//            var productRepoMock = new Mock<IProductRepository>();
//            var loggerMock = new Mock<ILogger<ProductController>>();
//            productRepoMock.Setup(repo => repo.Get(It.IsAny<string>())).ReturnsAsync(new Product { Id = OkProductId });
//            var controller = new ProductController(productRepoMock.Object, loggerMock.Object);

//            //act
//            var result = await controller.Get(OkProductId);

//            //assert
//            result.Should().BeOfType<ActionResult<Product>>()
//                .Which.Result.Should().BeOfType<OkObjectResult>()
//                .Which.Value.Should().BeOfType<Product>();
//        }

//        [Fact]
//        public async Task GetProductById_WhenProductByIdNull_ReturnNotFound()
//        {
//            //arrange
//            var NotProductId = "NotProductId";
//            var productRepoMock = new Mock<IProductRepository>();
//            var loggerMock = new Mock<ILogger<ProductController>>();
//            productRepoMock.Setup(repo => repo.Get(It.IsAny<string>())).ReturnsAsync((Product?)null);
//            var controller = new ProductController(productRepoMock.Object, loggerMock.Object);

//            //act
//            var result = await controller.Get(NotProductId);


//            //assert
//            result.Should().BeOfType<ActionResult<Product>>()
//                .Which.Result.Should().BeOfType<NotFoundResult>();
//        }

//        [Fact]
//        public async Task GetProductById_WhenExeption_Return500StatusCode()
//        {
//            //arrange
//            var ExceptionId = "ExceptionId";
//            var productRepoMock = new Mock<IProductRepository>();
//            var loggerMock = new Mock<ILogger<ProductController>>();
//            productRepoMock.Setup(repo => repo.Get(It.IsAny<string>())).ThrowsAsync(new Exception("ExceptionId"));
//            var controller = new ProductController(productRepoMock.Object, loggerMock.Object);

//            //act
//            var result = await controller.Get(ExceptionId);
//            //assert
//            result.Should().BeOfType<ActionResult<Product>>()
//                .Which.Result.Should().BeOfType<ObjectResult>()
//                .Which.StatusCode.Should().Be(500);
//        }


//        [Fact]
//        public async Task GetProductByBrand_ProductByBrandFound_ReturnsOkResult()
//        {
//            //arrange
//            var brandWithOkResult = "brandWithOkResult";
//            var products = new List<Product> { new Product { Id = "test", Brand = "BrandProduct" }, new Product { Id = "test", Brand = "BrandProduct" } };
//            var productRepoMock = new Mock<IProductRepository>();
//            var loggerMock = new Mock<ILogger<ProductController>>();
//            productRepoMock.Setup(repo => repo.GetByBrand(brandWithOkResult)).ReturnsAsync(products);
//            var controller = new ProductController(productRepoMock.Object, loggerMock.Object); // Создала экземпляр контроллера ProductsController, передавая фейковый repositoryMock в конструктор.

//            // act
//            var result = await controller.GetProductByBrand(brandWithOkResult);

//            // assert
//            result.Should().BeOfType<ActionResult<IEnumerable<Product>>>()
//                .Which.Result.Should().BeOfType<OkObjectResult>();
//        }

//        [Fact]
//        public async Task GetProductByBrand_WhenNoProducts_ReturnEmptyList()
//        {
//            //arrange
//            var brandProductsNull = "brandProductsNull";
//            var productRepoMock = new Mock<IProductRepository>();
//            var loggerMock = new Mock<ILogger<ProductController>>();
//            productRepoMock.Setup(repo => repo.GetByBrand(brandProductsNull)).ReturnsAsync((IEnumerable<Product>?)null);
//            var controller = new ProductController(productRepoMock.Object, loggerMock.Object);

//            // act
//            var result = await controller.GetProductByBrand(brandProductsNull);

//            //assert
//            result.Should().BeOfType<ActionResult<IEnumerable<Product>>>() // Проверяем, что контроллер возвращает ActionResult<IEnumerable<Product>>
//              .Which.Result.Should().BeOfType<OkObjectResult>() // Проверяем, что внутренний результат - это OkObjectResult
//              .Which.Value.Should().BeAssignableTo<IEnumerable<Product>>(); // Проверяем, что значение является коллекцией продуктов с ожидаемым типом значения
//            result.Value.Should().BeNull();
//        }

//        [Fact]
//        public async Task GetProductByBrand_WhenExeption_Return500StatusCode()
//        {
//            //arrange
//            var ExceptionBrand = "ExceptionBrand";
//            var productRepoMock = new Mock<IProductRepository>();
//            var loggerMock = new Mock<ILogger<ProductController>>();
//            productRepoMock.Setup(repo => repo.Get(It.IsAny<string>())).ThrowsAsync(new Exception("ExceptionBrand"));
//            var controller = new ProductController(productRepoMock.Object, loggerMock.Object);

//            //act
//            var result = await controller.Get(ExceptionBrand);
//            //assert
//            result.Should().BeOfType<ActionResult<Product>>()
//                .Which.Result.Should().BeOfType<ObjectResult>()
//                .Which.StatusCode.Should().Be(500);
//        }

//        [Fact]
//        public async Task GetProductByName_WhenProductByNameFound_ReturnOkResult()
//        {
//            //arrange
//            var okProductName = "OkProductName";
//            var productRepoMock = new Mock<IProductRepository>();
//            var loggerMock = new Mock<ILogger<ProductController>>();
//            productRepoMock.Setup(repo => repo.Get(It.IsAny<string>())).ReturnsAsync(new Product { Name = okProductName });
//            var controller = new ProductController(productRepoMock.Object, loggerMock.Object);

//            //act
//            var result = await controller.Get(okProductName);

//            //assert
//            result.Should().BeOfType<ActionResult<Product>>()
//                .Which.Result.Should().BeOfType<OkObjectResult>();
//        }



//        [Fact]
//        public async Task GetProductByName_WhenExeption_Return500StatusCode()
//        {
//            //arrange
//            var ExceptionName = "ExceptionName";
//            var productRepoMock = new Mock<IProductRepository>();
//            var loggerMock = new Mock<ILogger<ProductController>>();
//            productRepoMock.Setup(repo => repo.Get(It.IsAny<string>())).ThrowsAsync(new Exception("ExceptionName"));
//            var controller = new ProductController(productRepoMock.Object, loggerMock.Object);

//            //act
//            var result = await controller.Get(ExceptionName);
//            //assert
//            result.Should().BeOfType<ActionResult<Product>>()
//                .Which.Result.Should().BeOfType<ObjectResult>()
//                .Which.StatusCode.Should().Be(500);
//        }

//        [Fact]
//        public async Task Post_CreateNewProductSuccessfully_ReturnProduct()
//        {

//            // Arrange
//            var product = new Product();
//            var productRepoMock = new Mock<IProductRepository>();
//            var loggerMock = new Mock<ILogger<ProductController>>();
//            productRepoMock.Setup(repo => repo.Create(product)).Returns(Task.CompletedTask);
//            var controller = new ProductController(productRepoMock.Object, loggerMock.Object);

//            // act
//            var result = await controller.Post(product) as CreatedAtActionResult; // приведение результата выполнения метода Post к ожидаемым типам CreatedAtActionResult

//            //assert
//            result.Should().NotBeNull();
//            result.ActionName.Should().Be(nameof(ProductController.Get)); //  проверяем имя действия
//            result.Value.Should().BeEquivalentTo(product);
//        }

//        [Fact]
//        public async Task Post_CreateNewProduct_Return500StatusCode()
//        {
//            //arrange
//            var product = new Product();
//            var productRepoMock = new Mock<IProductRepository>();
//            var loggerMock = new Mock<ILogger<ProductController>>();
//            productRepoMock.Setup(repo => repo.Create(product)).ThrowsAsync(new Exception("Test failed to create user"));
//            var controller = new ProductController(productRepoMock.Object, loggerMock.Object);
//            //act
//            var result = await controller.Post(product);

//            //assert
//            result.Should().BeOfType<ObjectResult>()
//                .Which.StatusCode.Should().Be(500);
//        }

//        [Fact]
//        public async Task UpdateProduct_WhenProductExsist_ReturnsOkResult()
//        {
//            //arrange
//            var exisistinId = "exisistinId";
//            var updateProduct = new Product { Id = exisistinId, Name = "UpdateProductName" };
//            var productRepoMock = new Mock<IProductRepository>();
//            var loggerMock = new Mock<ILogger<ProductController>>();
//            productRepoMock.Setup(repo => repo.Get(exisistinId)).ReturnsAsync(new Product { Id = exisistinId, Name = "UpdateProductName" });
//            var controller = new ProductController(productRepoMock.Object, loggerMock.Object);
//            //act
//            var result = await controller.UpdateProduct(exisistinId, updateProduct);

//            //assert
//            result.Should().BeOfType<OkResult>();
//            productRepoMock.Verify(repo => repo.Update(updateProduct), Times.Once);//  проверяем был ли вызван метод Update для объекта productMockRepository с аргументами exisistinId один раз)
//        }

//        [Fact]
//        public async Task UpdateProduct_WhenProductNotExsist_ReturnNotFoundResult()

//        {
//            //arrange
//            var notExisistinId = "notExisistinId";
//            var updateProduct = new Product { Id = notExisistinId, Name = "NotUpdateProductName" };
//            var productRepoMock = new Mock<IProductRepository>();
//            var loggerMock = new Mock<ILogger<ProductController>>();
//            productRepoMock.Setup(repo => repo.Get(notExisistinId)).ReturnsAsync((Product?)null);
//            var controller = new ProductController(productRepoMock.Object, loggerMock.Object);

//            //act
//            var result = await controller.UpdateProduct(notExisistinId, updateProduct);
//            //assert
//            result.Should().BeOfType<NotFoundResult>();
//        }

//        [Fact]
//        public async Task UpdateProduct_WhenExeption_Return500StatusCode()
//        {
//            //arrange
//            var exeptionId = "exceptionId";
//            var updateProduct = new Product { Id = exeptionId, Name = "Exception" };
//            var productRepoMock = new Mock<IProductRepository>();
//            var loggerMock = new Mock<ILogger<ProductController>>();
//            productRepoMock.Setup(repo => repo.Get(exeptionId)).ThrowsAsync(new Exception());
//            var controller = new ProductController(productRepoMock.Object, loggerMock.Object);

//            //act
//            var result = await controller.UpdateProduct(exeptionId, updateProduct);
//            //assert
//            result.Should().BeOfType<ObjectResult>()
//                .Which.StatusCode.Should().Be(500);
//        }

//        [Fact]
//        public async Task DeleteProduct_WhenProductDelete_ReturnsOkResult()
//        {
//            //arrange
//            var deleteId = "deleteId";
//            var productRepoMock = new Mock<IProductRepository>();
//            var loggerMock = new Mock<ILogger<ProductController>>();
//            productRepoMock.Setup(repo => repo.Get(deleteId)).ReturnsAsync(new Product { Id = deleteId });
//            var controller = new ProductController(productRepoMock.Object, loggerMock.Object);

//            //act 
//            var result = await controller.DeleteProduct(deleteId);

//            //assert
//            result.Should().BeOfType<OkResult>();
//            productRepoMock.Verify(repo => repo.Delete(deleteId), Times.Once());
//        }

//        [Fact]
//        public async Task DeleteProduct_WhenProductIsNull_ReturnsNotFound()
//        {

//            //arrange
//            var deleteId = "deleteId";
//            var productRepoMock = new Mock<IProductRepository>();
//            var loggerMock = new Mock<ILogger<ProductController>>();
//            productRepoMock.Setup(repo => repo.Get(deleteId)).ReturnsAsync((Product?)null);
//            var controller = new ProductController(productRepoMock.Object, loggerMock.Object);

//            //act 
//            var result = await controller.DeleteProduct(deleteId);

//            //assert
//            result.Should().BeOfType<NotFoundResult>();
//        }

//        [Fact]
//        public async Task DeleteProduct_WhenExeption_Return500StatusCode()
//        {
//            //arrange
//            var deleteId = "deleteId";
//            var productRepoMock = new Mock<IProductRepository>();
//            var loggerMock = new Mock<ILogger<ProductController>>();
//            productRepoMock.Setup(repo => repo.Get(deleteId)).ThrowsAsync(new Exception());
//            var controller = new ProductController(productRepoMock.Object, loggerMock.Object);

//            //act
//            var result = await controller.DeleteProduct(deleteId);

//            //assert
//            result.Should().BeOfType<ObjectResult>()
//                .Which.StatusCode.Should().Be(500);
//        }
//    }
//}

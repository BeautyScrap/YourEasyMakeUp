using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourEasyRent.Controllers;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Entities;
using YourEasyRent.Services;

namespace YourEasyRentTest.Controllers
{
    public class ListingControllerTest
    {

        [Fact]
        public async Task GetProducts_WhenAllClientsReturnProucts_ReturnsAllListings()
        {
            // arrange
            var productRepoMock = new Mock<IProductRepository>();
            var mockClients = new List<Mock<IProductsSiteClient>>();
            for (int i = 0; i < 3; i++)
            {
                var mockClient = new Mock<IProductsSiteClient>();
                mockClients.Add(mockClient);
            }
            var controller = new ListingController(productRepoMock.Object, mockClients.Select(c => c.Object));
            foreach (var mockClient in mockClients)
            {
                mockClient.Setup(x => x.FetchFromSectionAndPage(It.IsAny<Section>(), It.IsAny<int>()))
                .ReturnsAsync(new List<Product>()
                {
                    new Product {  Name = "Product 1" },
                    new Product { Id = 2, Name = "Product 2" },
                });
                var expectedProducts = new List<Product>()
                 {
                    new Product { Id = 1, Name = "Product 1" },
                    new Product { Id = 2, Name = "Product 2" },
                    new Product { Id = 1, Name = "Product 1" },
                    new Product { Id = 2, Name = "Product 2" },
                    new Product { Id = 1, Name = "Product 1" },
                    new Product { Id = 2, Name = "Product 2" }
                };
                productRepoMock.Setup(repository => repository.UpsertManyProducts(It.IsAny<IEnumerable<Product>>()));

                //Act
                var result = await controller.GetProducts();

                // Assert
                result.Should().NotBeNull();
                result.Should().HaveCount(expectedProducts.Count);
                result.Should().BeEquivalentTo(expectedProducts);
            }
        }
    }
}

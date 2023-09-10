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
        public ListingControllerTest()
        {
                
        }

        [Fact]
        public async Task GetProducts_WhenAllClientsReturnProucts_ReturnsCombinedProuctsLiist()
        {
            // arrange
            var productRepoMock = new Mock<IProductRepository>();
            var douglasClient = new Mock<IDouglasProductSiteClient>();
            douglasClient.Setup(
                x => x.FetchFromDouglasSection(It.IsAny<Section>(), It.IsAny<int>()))
                .ReturnsAsync(new List<Product>()
            {
                new Product()
                {
                    Name = "Test1",
                },
                new Product()
                {
                    Name = "Test2",
                }
            });

            var sephoraClient = new Mock<ISephoraProductSiteClient>();
            sephoraClient.Setup(
            x => x.FetchFromSephoraSection(It.IsAny<Section>(), It.IsAny<int>()))
            .ReturnsAsync(new List<Product>(){

                new Product()
                {
                    Name = "Test3",
                }
            });

            var controller = new ListingController(productRepoMock.Object, douglasClient.Object, sephoraClient.Object);
            
            // act
            var result = await controller.GetProducts();

            // assert
            var expected = new List<Product>()
            {
                new Product()
                {
                    Name = "Test3",
                },
                new Product()
                {
                    Name = "Test1",
                },
                new Product()
                {
                    Name = "Test2",
                },

            };

            result.Should().BeEquivalentTo(expected);

            productRepoMock.Verify(x=> x.CreateMany(It.IsAny<IEnumerable<Product>>()));
        }
    }
}

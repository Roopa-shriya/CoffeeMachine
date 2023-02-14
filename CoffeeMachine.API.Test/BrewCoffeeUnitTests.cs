using Microsoft.AspNetCore.Mvc;
using CoffeeMachine.API.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using CoffeeMachine.API.Interfaces;
using Moq;
using CoffeeMachine.API.Models;

namespace CoffeeMachine.API.Test
{
    [TestFixture]
    public class BrewCoffeeUnitTests
    {

        private BrewCoffeeController coffeeController;
        private Mock<ICofffeService> mockCofffeService;

        [SetUp]
        public void SetUp()
        {
            mockCofffeService = new Mock<ICofffeService>();
            coffeeController = new BrewCoffeeController(mockCofffeService.Object);
        }

        [Test]
        public void BrewCoffee_Returns_200_Ok()
        {
            // Arrange
            mockCofffeService.SetupGet(x => x.CoffeeCount).Returns(1);
            mockCofffeService.SetupGet(x => x.PreparedTime).Returns(new DateTime(2023, 02, 13, 11, 56, 24));
            mockCofffeService.Setup(x => x.GetCoffee()).Returns(new BrewCoffeeResponse
            {
                message = "Your piping hot coffee is ready",
                prepared = new DateTime(2023, 02, 13, 11, 56, 24).ToString("yyyy-MM-ddTHH:mm:sszz00")
            });

            // Act
            var result = coffeeController.BrewCoffee() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);

            result.Value.Should().BeEquivalentTo(new
            {
                message = "Your piping hot coffee is ready",
                prepared = new DateTime(2023, 02, 13, 11, 56, 24).ToString("yyyy-MM-ddTHH:mm:sszz00")
            });

        }

        [Test]
        public void BrewCoffee_Returns_503_ServiceUnavailable()
        {
            // Arrange
            mockCofffeService.SetupGet(x => x.CoffeeCount).Returns(5);


            // Act
            var result = coffeeController.BrewCoffee() as StatusCodeResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status503ServiceUnavailable);
        }

        [Test]
        public void BrewCoffee_Returns_418_ImATeapot()
        {
            // Arrange
            mockCofffeService.SetupGet(x => x.CoffeeCount).Returns(1);
            mockCofffeService.SetupGet(x => x.PreparedTime).Returns(new DateTime(2023, 04, 1));


            // Act
            var result = coffeeController.BrewCoffee() as StatusCodeResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status418ImATeapot);
        }
    }
}





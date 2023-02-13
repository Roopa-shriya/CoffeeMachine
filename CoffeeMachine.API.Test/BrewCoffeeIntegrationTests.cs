using System.Net;
using CoffeeMachine.API.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;

namespace CoffeeMachine.API.Test
{
    [TestFixture]
    public class BrewCoffeeIntegrationTests 
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _client = new WebApplicationFactory<BrewCoffeeController>().CreateDefaultClient();
        }

        [Test]
        public async Task BrewCoffee_Returns200Ok()
        {
            // Arrange
            
            // Act
            var response = await _client.GetAsync("/brew-coffee");

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseString);

            responseJson["message"].Value<string>().Should().Be("Your piping hot coffee is ready");
            responseJson["prepared"].Value<string>().Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        public async Task BrewCoffee_Returns503ServiceUnavailable()
        {
            // Arrange
            for (int i = 0; i < 4; i++)
            {
                var response = await _client.GetAsync("/brew-coffee");
                response.EnsureSuccessStatusCode();
            }

            // Act
            var finalResponse = await _client.GetAsync("/brew-coffee");

            // Assert
            finalResponse.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
        }

        //This test will be valid only for April 1st
        [Test]
        public async Task BrewCoffee_Returns418ImATeapot()
        {
            // Arrange
            if (DateTime.Now.Month == 4 && DateTime.Now.Day == 1)
            {
                // Act
                var response = await _client.GetAsync("/brew-coffee");

                // Assert
                response.StatusCode.Should().Be((HttpStatusCode)418);
            }
        }
    }
}

using CoffeeMachine.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.API.Controllers
{
    [Route("/brew-coffee")]
    [ApiController]
    public class BrewCoffeeController : ControllerBase
    {
        private readonly ICofffeService _coffeeService;

        public BrewCoffeeController(ICofffeService coffeeService)
        {
            _coffeeService= coffeeService;
        }


        [HttpGet]
        public IActionResult BrewCoffee()
        {       
            _coffeeService.UpdateCoffeeCount();

            if (_coffeeService.PreparedTime.Month == 4 && _coffeeService.PreparedTime.Day == 1)
            {
                return StatusCode(StatusCodes.Status418ImATeapot);
            }

            if (_coffeeService.CoffeeCount % 5 == 0)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            var response = new
            {
                message = "Your piping hot coffee is ready",
                prepared = _coffeeService.PreparedTime.ToString("yyyy-MM-ddTHH:mm:sszz00")
            };

            return Ok(response);
        }
    }
}

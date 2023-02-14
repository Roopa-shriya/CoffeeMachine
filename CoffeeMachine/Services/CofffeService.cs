using CoffeeMachine.API.Interfaces;
using CoffeeMachine.API.Models;

namespace CoffeeMachine.API.Services
{
    public class CofffeService : ICofffeService
    {
        public int CoffeeCount { get; set; }
        public DateTime PreparedTime { get; set; }

        public void UpdateCoffeeCount()
        {
            CoffeeCount++;
            PreparedTime = DateTime.Now;
        }

       public BrewCoffeeResponse GetCoffee()
       {
            BrewCoffeeResponse brewCoffeeResponse = new()
            {
                message = "Your piping hot coffee is ready",
                prepared = PreparedTime.ToString("yyyy-MM-ddTHH:mm:sszz00")
            };
            
            return brewCoffeeResponse;
        }
    }
}

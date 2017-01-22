using System;
using Microsoft.Extensions.Configuration;


namespace WebApplication.Services
{
    public class Greeter : IGreeter
    {
        private string _greeting;

        // An instance that implements IConfiguration will be injected by the container
        public Greeter(IConfiguration configuration)
        {
            _greeting = configuration["greeting"];
        }
        public string GetGreeting()
        {
            return _greeting;
        }
    }
}
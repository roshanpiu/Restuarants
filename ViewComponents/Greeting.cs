using Microsoft.AspNetCore.Mvc;
using WebApplication.Services;

namespace WebApplication.ViewComponents
{
    public class Greeting : ViewComponent
    {
        private IGreeter _greeter;

        public Greeting(IGreeter greeter)
        {
            _greeter = greeter;
        }

        public IViewComponentResult Invoke()
        {
            var model = _greeter.GetGreeting();
            //if the model object is a string asp.net will see it as the view name not the model object
            return View("Default", model);
        }
    }
}
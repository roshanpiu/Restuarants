using Microsoft.AspNetCore.Mvc;
using WebApplication.Entities;
using WebApplication.ViewModels;
using WebApplication.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication.Controllers
{
    
    //The controller base class gives aditional capabilities
    [Authorize]
    public class HomeController : Controller
    {
        private IRestaurantData _restaurantData;
        private IGreeter _greeter;
        public HomeController(IRestaurantData restaurantData, IGreeter greeter)
        {
            _restaurantData = restaurantData;
            _greeter = greeter;
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            //return File() returns a File
            //return View() returns a View
            //return Content("Hello")
            //return new ObjectResult(obj) // by default serializes the object to json
            //All these result types implemets IActionResult interface
            //The Controller only has to decide what to return framework handles the rest

            var model = new HomePageViewModel();
            model.Restaurants = _restaurantData.GetAll();
            model.CurrentGreeting = _greeter.GetGreeting();
            
            //to use the Razor view engine of ASP.NET the controller has to return a View()
            //the View carries the name of the view that we want to use which will be a file on file system
            //and View can also carry along a model object to the view
            //when ASP.NET sees your controller action carries a view results it will execute the view on file system
            //which will produce the html that gets rendered on client side

            //By default ASP.NET will look for a view at /Views/Home/index.cshtml
            //Then at /Views/Shared/index.cshtml
            //explicitly specify the view name View("name", model)
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var model = _restaurantData.Get(id);
            if(model == null) 
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(RestaurantEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                var restaurant = new Restaurant();
                restaurant.Name = model.Name;
                restaurant.Cuisine = model.Cuisine;

                _restaurantData.Add(restaurant);
                _restaurantData.Commit();
                //responding to a post request directly can cause trouble because user can refresh and data will be submitted twice
                //when there is a successfull post you need to respond with a http redirect and tell the browser to send a get request
                //to read data from somewhere else following this pattern we can avoid multiple posts
                // return View("Details", restaurant); responding like this to a successfull post request is a bad practice
                
                return RedirectToAction("Details", new { id = restaurant.Id });
            }
            else
            {
                return View();
            }
            
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _restaurantData.Get(id);
            if(model == null) 
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, RestaurantEditViewModel input)
        {
            var restaurant = _restaurantData.Get(id);
            if(restaurant != null && ModelState.IsValid)
            {
                restaurant.Name = input.Name;
                restaurant.Cuisine = input.Cuisine;
                _restaurantData.Commit();
                return RedirectToAction("Details", new { id = restaurant.Id });
            }
            return View(restaurant);
        }
    }
}
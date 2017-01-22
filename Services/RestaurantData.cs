using System.Collections.Generic;
using WebApplication.Entities;
using System.Linq;

namespace WebApplication.Services
{
    public interface IRestaurantData
    {
        IEnumerable<Restaurant> GetAll();
        Restaurant Get(int id);
        void Add(Restaurant newRestaurant);
    }

    public class SqlRestaurantData : IRestaurantData
    {
        private WebApplicationDbContext _context;

        public SqlRestaurantData(WebApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Restaurant> GetAll()
        {
            return _context.Restaurants;
        }

        public Restaurant Get(int id)
        {
            return _context.Restaurants.FirstOrDefault( r => r.Id == id);
        }

        public void Add(Restaurant newRestaurant)
        {
            _context.Add(newRestaurant);
            _context.SaveChanges();
        }
    }

    public class InMemoryRestaurantData : IRestaurantData
    {
        //Lists are not thread safe carefull when using in a web application
        //can not be used by concurrent users
        static List<Restaurant> _restaurants;

        public InMemoryRestaurantData()
        {
            _restaurants = new List<Restaurant>
            {
                new Restaurant { Id = 1, Name = "Chinese Dragon"},
                new Restaurant { Id = 2, Name = "Nirmala"},
                new Restaurant { Id = 3, Name = "Master Wok"},
                new Restaurant { Id = 4, Name = "Rathu kakulu"},

            };
        }
        public IEnumerable<Restaurant> GetAll()
        {
            return _restaurants;
        }

        public Restaurant Get(int id)
        {
            return _restaurants.FirstOrDefault(r => r.Id == id);
        }

        public void Add(Restaurant newRestaurant)
        {
            newRestaurant.Id = _restaurants.Max( r => r.Id) + 1;
            _restaurants.Add(newRestaurant);
        }
    }
}
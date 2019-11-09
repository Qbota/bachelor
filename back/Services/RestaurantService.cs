using Backend.Entities;
using Backend.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class RestaurantService : IRestaurantService
    {
        private IRepository<Restaurant> _restaurantRepo;
        private IRepository<Comment> _commentRepo;
        public RestaurantService(IRepository<Restaurant> restaurantRepository, IRepository<Comment> commentRepository)
        {
            _restaurantRepo = restaurantRepository;
            _commentRepo = commentRepository;
        }
        public IEnumerable<Restaurant> GetAll() => _restaurantRepo.GetAll();
        public Restaurant GetRestaurantById(int id) => _restaurantRepo.GetOne(id);
        public Restaurant GetRestaurantByName(string name) => _restaurantRepo.GetOne(name);       
        public IEnumerable<Restaurant> GetRestaurantsWithMeal(string meal) => 
            _restaurantRepo.GetAll().Where(x => x.HasMeal(meal) == true).Select(x => { x.Menu = x.Menu.Where(m => m.Name == meal); return x; });

        public Restaurant RegisterRestaurant(Restaurant restaurant) => _restaurantRepo.Create(restaurant);

        public IEnumerable<Meal> UpdateMenu(string name, List<Meal> menu)
        {
            var updated = _restaurantRepo.Update(new Restaurant()
            {
                Name = name,
                Menu = menu
            });
            return updated.Menu;
        }

        public IEnumerable<Meal> GetMenu(string name)
        {
            return _restaurantRepo.GetOne(name).Menu;
        }

        public IEnumerable<Meal> GetMeals()
        {
            var restaurants = _restaurantRepo.GetAll();
            var meals = new List<Meal>();
            foreach(var restaurant in restaurants)
            {
                foreach(var meal in restaurant.Menu)
                {
                    meals.Add(meal);
                }
            }
            return meals;
        }

        public Comment AddComment(Comment comment)
        {
            return _commentRepo.Create(comment);
        }
    }
}

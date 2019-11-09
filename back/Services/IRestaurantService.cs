using Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IRestaurantService
    {
        IEnumerable<Restaurant> GetAll();
        Restaurant GetRestaurantById(int id);
        Restaurant GetRestaurantByName(string name);
        IEnumerable<Restaurant> GetRestaurantsWithMeal(string meal);
        Restaurant RegisterRestaurant(Restaurant restaurant);
        IEnumerable<Meal> UpdateMenu(string name, List<Meal> menu);
        IEnumerable<Meal> GetMenu(string name);
        IEnumerable<Meal> GetMeals();
        Comment AddComment(Comment comment);

    }
}

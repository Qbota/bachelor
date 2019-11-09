using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Entities
{
    public interface IRestaurantContext
    {
        IEnumerable<Restaurant> GetRestaurants();
        Restaurant InsertRestaurant(Restaurant restaurant);
        Restaurant UpdateRestaurant(Restaurant restaurant);
    }
}

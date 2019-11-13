using Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Repos
{
    public class RestaurantRepository : IRepository<Restaurant>
    {
        public IRestaurantContext _context { get; set; }
        public RestaurantRepository(IRestaurantContext context)
        {
            _context = context;
        }
        public Restaurant Create(Restaurant entity) => _context.InsertRestaurant(entity);
       

        public Restaurant Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Restaurant> GetAll() => _context.GetRestaurants().Where(x => x.IsActive == true);


        public Restaurant GetOne(int id) => _context.GetRestaurants().Where(x => x.Id == id).FirstOrDefault();
        

        public Restaurant GetOne(string name) => _context.GetRestaurants().Where(x => x.Name == name).FirstOrDefault();

        public Restaurant Update(Restaurant entity)
        {
            var old = _context.GetRestaurants().Where(x => x.Name == entity.Name).FirstOrDefault();
            var temp = entity;
            //0 or null means no changes in that field
            if (entity.Lat != 0)
                old.Lat = entity.Lat;
            if (entity.Lng != 0)
                old.Lng = entity.Lng;
            
            old.Menu = entity.Menu;
            return _context.UpdateRestaurant(old);
        }
    }
}

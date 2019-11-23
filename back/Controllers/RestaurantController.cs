using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Backend.Entities;
using Backend.Services;
using Backend.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _service;
        private readonly ILogger _logger;
        public RestaurantController(IRestaurantService service, ILogger logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpGet]
        [Route("Restaurant")]
        public ActionResult<IEnumerable<Restaurant>> GetRestaurantsNames()
        {
            var list = _service.GetAll().Select(x => x.Name);
            return Ok(list);
        }
        [HttpGet]
        [Route("GetAllRestaurants")]
        public ActionResult<IEnumerable<Restaurant>> GetAllRestaurants()
        {
            _logger.AddInfoLog("Received http get request to get all restaurants");
            return Ok(_service.GetAll());
        }
        [HttpGet]
        [Route("Restaurant/{name}")]
        public ActionResult<Restaurant> GetSingleRestaurantByName([FromRoute] string name)
        {
            var restaurant = _service.GetRestaurantByName(name);
            if(restaurant == null)
            {
                return NotFound();
            }
            restaurant.Menu = null;
            return Ok(restaurant);
        }
        [Authorize(Policy ="owner")]
        [HttpGet]
        [Route("Restaurant/menu")]
        public ActionResult<IEnumerable<Meal>> GetUsersRestaurant()
        {
            var name = GetNameFromRequest(Request);
            if(name == null)
            {
                _logger.AddInfoLog("Unauthorized access to menu of restaurant");
                return Unauthorized();
            }
            return Ok(_service.GetRestaurantByName(name).Menu);
        }
        [Authorize(Policy = "owner")]
        [HttpPut]
        [Route("Restaurant/menu")]
        public ActionResult<IEnumerable<Meal>> UpdateMenu(List<Meal> menu)
        {
            var name = GetNameFromRequest(Request);
            if (name == null)
            {
                _logger.AddInfoLog("Unauthorized access to changing menu of restaurant");
                return Unauthorized();
            }
            return Ok(_service.UpdateMenu(name, menu));
        }
        [HttpGet]
        [Route("Restaurant/{name}/menu")]
        public ActionResult<IEnumerable<Meal>> GetSingleRestaurantByNameMenu([FromRoute] string name)
        {
            return Ok(_service.GetRestaurantByName(name).Menu);
        }
        [Authorize(Policy = "admin")]
        [HttpPut]
        [Route("Restaurant/{name}/menu")]
        public ActionResult<IEnumerable<Meal>> UpdateMenu([FromRoute] string name, List<Meal> menu)
        {
            return Ok(_service.UpdateMenu(name, menu));
        }
        [HttpGet]
        [Route("Restaurant/{name}/coordinates")]
        public ActionResult<Restaurant> GetCoordinatesOfRestaurant([FromRoute] string name)
        {
            var restaurant = _service.GetRestaurantByName(name);
            return Ok(new { lat = restaurant.Lat, lng = restaurant.Lng });
        }
        [HttpGet]
        [Route("GetPlacesWithMeal/{meal}")]
        public ActionResult<IEnumerable<Restaurant>> GetRestaurantsWithMeal([FromRoute] string meal)
        {
            return Ok(_service.GetRestaurantsWithMeal(meal));
        }
        [HttpGet]
        [Route("Meal")]
        public ActionResult<IEnumerable<Meal>> GetMeals()
        {
            return Ok(_service.GetMeals());
        }
        [Authorize(Policy = "user")]
        [HttpPost]
        [Route("Meal/Comment")]
        public ActionResult<Comment> AddComment(Comment comment)
        {
            return Ok(_service.AddComment(comment));
        }
        [HttpPost]
        [Route("Restaurant")]
        public ActionResult<Restaurant> RegisterRestaurant(Restaurant restaurant)
        {
           return Ok(_service.RegisterRestaurant(restaurant));
        }
        private string GetNameFromRequest(HttpRequest request)
        {
            try
            {
                var jwt = Request.Headers["Authorization"].ToString().Split(" ")[1];
                var handler = new JwtSecurityTokenHandler();
                return handler.ReadJwtToken(jwt).Claims.Where(x => x.Type == "unique_name").FirstOrDefault().Value;
            }
            catch
            {
                return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Tools;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Backend.Entities
{
    public class RestaurantContext : IRestaurantContext
    {
        private string ConnectionString;
        private IConfiguration _config;
        private readonly ILogger _logger;
        public RestaurantContext(IConfiguration configuration, ILogger logger)
        {
            _logger = logger;
            _config = configuration;
            ConnectionString = _config.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
        public Restaurant InsertRestaurant(Restaurant restaurant)
        {
            using(var conn = GetConnection())
            {
                _logger.AddInfoLog("Establishing connection to dabatase");
                try
                {
                    _logger.AddInfoLog("");
                    conn.Open();
                    var cmd = new MySqlCommand($"insert into restaurants values({restaurant.ToString()})", conn);
                    cmd.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    _logger.AddErrorLog($"Error while inserting new restaurant: {restaurant.Name}" + e.Message);
                }
                
            }
            return restaurant;
        }
        public IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>();
            var meals = new List<Meal>();
            var comments = new List<Comment>();
            using (var conn = GetConnection())
            {
                _logger.AddInfoLog("Establishing connection to dabatase");
                try
                {
                    _logger.AddInfoLog("Getting restaurant data from database");
                    conn.Open();
                    var cmd = new MySqlCommand("select * from restaurants", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var temp = reader;
                            restaurants.Add(new Restaurant()
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = reader["restaurant_name"].ToString(),
                                Lat = Convert.ToDouble(reader["restaurant_lat"]),
                                Lng = Convert.ToDouble(reader["restaurant_lng"]),
                                IsActive = Convert.ToBoolean(reader["is_active"])

                            });
                        }
                    }
                    cmd = new MySqlCommand("SELECT meals.meal_id, meals.meal_name, meals.meal_price, meals.restaurant_id, avg(comments.comment_rating) " +
                        "FROM meals left join comments on meals.meal_id=comments.meal_id " +
                        "group by meal_id", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var meal = new Meal()
                            {
                                Id = Convert.ToInt32(reader["meal_id"]),
                                Name = reader["meal_name"].ToString(),
                                Price = Convert.ToDouble(reader["meal_price"]),
                                RestaurantId = Convert.ToInt32(reader["restaurant_id"])
                            };
                            try
                            {
                                meal.AverageRate = Convert.ToDouble(reader["avg(comments.comment_rating)"]);
                            }
                            catch
                            {
                                meal.AverageRate = 0;
                            }
                            meals.Add(meal);
                        }
                    }

                    cmd = new MySqlCommand("select * from comments", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comments.Add(new Comment()
                            {
                                MealId = Convert.ToInt32(reader["meal_id"]),
                                Text = reader["comment_text"].ToString(),
                                Rate = Convert.ToInt32(reader["comment_rating"])
                            });
                        }

                    }

                    foreach (var restaurant in restaurants)
                    {
                        restaurant.Menu = meals.Where(x => x.RestaurantId == restaurant.Id);
                        foreach (var meal in restaurant.Menu)
                        {
                            meal.Comments = comments.Where(x => x.MealId == meal.Id).Select(x => x.Text);
                            meal.Rates = comments.Where(x => x.MealId == meal.Id).Select(x => x.Rate);
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.AddErrorLog("Error while getting restaurants data from database" + e.Message);
                }

                return restaurants;
            }
        }

        public Restaurant UpdateRestaurant(Restaurant restaurant)
        {
            using (var conn = GetConnection())
            {
                _logger.AddInfoLog("Establishing connection to dabatase");
                try
                {
                    _logger.AddInfoLog($"Trying to update restaurant: {restaurant.Name} data");
                    conn.Open();
                    var cmd = new MySqlCommand($"update restaurants " +
                        $"set restaurant_lat={restaurant.Lat.ToString(System.Globalization.CultureInfo.InvariantCulture)}, " +
                        $"restaurant_lng={restaurant.Lng.ToString(System.Globalization.CultureInfo.InvariantCulture)} " +
                        $"where id={restaurant.Id} and restaurant_name like \"{restaurant.Name}\";", conn);
                    //cmd.ExecuteNonQuery();

                    cmd = new MySqlCommand($"select * from meals where restaurant_id={restaurant.Id}", conn);
                    var oldMenu = new List<Meal>();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var meal = new Meal()
                            {
                                Id = Convert.ToInt32(reader["meal_id"]),
                                Name = reader["meal_name"].ToString(),
                                Price = Convert.ToDouble(reader["meal_price"]),
                                RestaurantId = Convert.ToInt32(reader["restaurant_id"])
                            };

                            oldMenu.Add(meal);
                        }
                    }
                    if (oldMenu.Count() > restaurant.Menu.Count())
                    {
                        for (int i = 0; i < oldMenu.Count(); i++)
                        {
                            if (i < restaurant.Menu.Count())
                            {
                                var meal = restaurant.Menu.ToList()[i];
                                cmd = new MySqlCommand($"update meals set " +
                                $"meal_name=\"{meal.Name}\", " +
                                $"meal_price={meal.Price.ToString(System.Globalization.CultureInfo.InvariantCulture)} " +
                                $"where meal_id={meal.Id} and restaurant_id={meal.RestaurantId};", conn);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                var meal = oldMenu.ToList()[i];
                                cmd = new MySqlCommand($"delete from meals where meal_id={meal.Id}", conn);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    else if (oldMenu.Count() < restaurant.Menu.Count())
                    {
                        for (int i = 0; i < restaurant.Menu.Count(); i++)
                        {
                            var meal = restaurant.Menu.ToList()[i];
                            if (i < oldMenu.Count())
                            {
                                cmd = new MySqlCommand($"update meals set " +
                                $"meal_name=\"{meal.Name}\", " +
                                $"meal_price={meal.Price.ToString(System.Globalization.CultureInfo.InvariantCulture)} " +
                                $"where meal_id={meal.Id} and restaurant_id={meal.RestaurantId};", conn);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmd = new MySqlCommand($"insert into meals values" +
                                    $"(0,\"{meal.Name}\",{meal.Price.ToString(System.Globalization.CultureInfo.InvariantCulture)},{meal.RestaurantId});", conn);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                    {
                        //just update
                        foreach (var meal in restaurant.Menu)
                        {
                            cmd = new MySqlCommand($"update meals set " +
                                $"meal_name=\"{meal.Name}\", " +
                                $"meal_price={meal.Price.ToString(System.Globalization.CultureInfo.InvariantCulture)} " +
                                $"where meal_id={meal.Id} and restaurant_id={meal.RestaurantId};", conn);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.AddErrorLog($"Error while updating restaurant: {restaurant.Name} data" + e.Message);
                }
                
                
            }
            return restaurant;
        }
    }
}

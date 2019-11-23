using Backend.Tools;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Entities
{
    public class UserContext : IUserContext
    {
        public string ConnectionString;
        private IConfiguration _config;
        private ILogger _logger;

        public UserContext(IConfiguration config, ILogger logger)
        {
            _logger = logger;
            _config = config;
            ConnectionString = _config.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
        }

        public User AddUser(User user)
        {

            using (var conn = GetConnection())
            {
                try
                {
                    _logger.AddInfoLog("Establishing connection with database");
                    conn.Open();
                    _logger.AddInfoLog($"Inserting new user: {user.ToString()} to database");
                    var cmd = new MySqlCommand($"insert into users values({user.ToString()})", conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    _logger.AddErrorLog($"Error while inserting new user: {user.ToString()}" + e.Message);
                }

            }
            return user;
        }

        public IEnumerable<User> GetAllUsers()
        {
            var list = new List<User>();
            using(var conn = GetConnection())
            {
                try
                {
                    _logger.AddInfoLog("Establishing connection with database");
                    conn.Open();
                    _logger.AddInfoLog("Getting users data from database");
                    var cmd = new MySqlCommand("select * from users", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new User()
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Email = reader["email"].ToString(),
                                Password = reader["password"].ToString(),
                                IsAdmin = Convert.ToBoolean(reader["is_admin"]),
                                RestaurantId = Convert.ToInt32(reader["restaurant_id"]),
                                IsActive = Convert.ToBoolean(reader["is_active"])
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.AddErrorLog("Error while getting users from database" + e.Message);
                }
                
            }
            return list;
        }
        public int SwitchActiveUser(int id)
        {
            using(var conn = GetConnection())
            {
                try
                {
                    _logger.AddInfoLog("Establishing connection with database");
                    conn.Open();
                    _logger.AddInfoLog($"Changing status of user: {id}");
                    var cmd = new MySqlCommand($"Update users set is_active = !is_active where id={id}", conn);
                    cmd.ExecuteNonQuery();
                    _logger.AddInfoLog($"Changing status of restaurant of user: {id}");
                    cmd = new MySqlCommand($"update restaurants set is_active = !is_active where id=(select restaurant_id from users where id={id})", conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    _logger.AddErrorLog($"Error while changing status of user: {id}" + e.Message);
                }
                
            }

            return id;
        }
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

    }
}

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

        public UserContext(IConfiguration config)
        {
            _config = config;
            ConnectionString = _config.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
        }

        public User AddUser(User user)
        {
            var temp = $"insert into users values({user.ToString()})";
            using (var conn = GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand($"insert into users values({user.ToString()})", conn);
                cmd.ExecuteNonQuery();
            }
            return user;
        }

        public IEnumerable<User> GetAllUsers()
        {
            var list = new List<User>();
            using(var conn = GetConnection())
            {
                var cmd = new MySqlCommand("select * from users", conn);
                conn.Open();
                using(var reader = cmd.ExecuteReader())
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
            return list;
        }
        public int SwitchActiveUser(int id)
        {
            using(var conn = GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand($"Update users set is_active = !is_active where id={id}", conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand($"update restaurants set is_active = !is_active where id=(select restaurant_id from users where id={id})", conn);
                cmd.ExecuteNonQuery();
            }

            return id;
        }
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

    }
}

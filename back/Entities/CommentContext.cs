using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Entities
{
    public class CommentContext : ICommentContext
    {
        private string ConnectionString;
        private IConfiguration _config;
        public CommentContext(IConfiguration configuration)
        {
            _config = configuration;
            ConnectionString = _config.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
        }
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public Comment InsertComment(Comment comment)
        {
            using(var conn = GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand($"insert into comments values({comment.ToString()})", conn);
                cmd.ExecuteNonQuery();
            }
            return comment;
        }
    }
}

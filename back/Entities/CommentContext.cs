using Backend.Tools;
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
        private ILogger _logger;
        public CommentContext(IConfiguration configuration, ILogger logger)
        {
            _config = configuration;
            _logger = logger;
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
                _logger.AddInfoLog("Establishing connection to database");
                try
                {
                    conn.Open();
                    var cmd = new MySqlCommand($"insert into comments values({comment.ToString()})", conn);
                    _logger.AddInfoLog("Inserting new comment into database");
                    cmd.ExecuteNonQuery();
                }catch(Exception e)
                {
                    _logger.AddErrorLog("Error while inserting comennt into database" + e.Message);
                }
            }
            return comment;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Backend.Entities;
using Backend.Repos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services
{
    public class UserService : IUserService
    {
        IRepository<User> _userRepo;
        IRepository<Restaurant> _restaurantRepo;
        IConfiguration _config;
        private string secret;
        public UserService(IRepository<User> userRepository, IRepository<Restaurant> restaurantRepository, IConfiguration config)
        {
            _userRepo = userRepository;
            _restaurantRepo = restaurantRepository;
            _config = config;
            secret = _config.GetSection("Secrets").GetSection("JWT").Value;
        }
        public IEnumerable<User> GetUsers() => _userRepo.GetAll();

        public User Register(User user)
        {
            user.IsAdmin = false;
            user.Password = ComputeSha256Hash(user.Password);
            var created = _userRepo.Create(user);
            created.Password = "";
            //validation
            return created;
        }

        public User ValidateLogin(string email, string password)
        {
            var repoUser = _userRepo.GetOne(email);
            if(repoUser == null)
            {
                return new User();
            }
            if (repoUser.Password == ComputeSha256Hash(password))
            {

                //authenticated
                repoUser.Password = "";
                try
                {
                    repoUser.Restaurant = _restaurantRepo.GetOne(repoUser.RestaurantId);
                }
                catch
                {
                    repoUser.Restaurant = null; 
                }
                repoUser.Token = GenerateToken(repoUser);
                return repoUser;
            }
            else
            {
                //not authenticated
                return new User();
            }
        }

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                var builder = new StringBuilder();

                foreach (var b in bytes){
                    builder.Append(b.ToString("x2"));
                }
                
                return builder.ToString();
            }
        }

        private string GenerateToken(User user)
        {
            var role = "";
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor();
            if (user.IsAdmin)
            {
                role = "admin";
                tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
               {
                    new Claim(ClaimTypes.Role, role),
               }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
            }
            else if(user.RestaurantId == 0)
            {
                role = "user";
                tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
               {
                    new Claim(ClaimTypes.Role, role),
               }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
            }
            else
            {
                role = "owner";
                tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
               {
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.Name, user.Restaurant.Name)
               }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
            }
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

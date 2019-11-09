using Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        User ValidateLogin(string email, string password);
        User Register(User user);
    }
}

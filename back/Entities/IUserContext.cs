using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Entities
{
    public interface IUserContext
    {
        User AddUser(User user);
        IEnumerable<User> GetAllUsers();
        int SwitchActiveUser(int id);
    }
}

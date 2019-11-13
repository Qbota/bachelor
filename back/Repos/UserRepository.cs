using Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Repos
{
    public class UserRepository : IRepository<User>
    {
        private IUserContext _userContext;
        public UserRepository(IUserContext context)
        {
            _userContext = context;
        }
        public User Create(User entity) => _userContext.AddUser(entity);

        public User Delete(int id)
        {
            _userContext.SwitchActiveUser(id);
            return GetOne(id);
        }

        public IEnumerable<User> GetAll() => _userContext.GetAllUsers();

        public User GetOne(int id) => _userContext.GetAllUsers().Where(x => x.Id == id).FirstOrDefault();

        public User GetOne(string name) => _userContext.GetAllUsers().Where(x => x.Email == name).FirstOrDefault();

        public User Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}

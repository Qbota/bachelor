using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Repos
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetOne(int id);
        T GetOne(string name);
        T Create(T entity);
        T Delete(int id);
        T Update(T entity);
    }
}

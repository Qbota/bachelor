using Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Repos
{
    public class CommentRepository : IRepository<Comment>
    {
        private ICommentContext _context;
        public CommentRepository(ICommentContext context)
        {
            _context = context;
        }
        public Comment Create(Comment entity)
        {
            return _context.InsertComment(entity);
        }

        public Comment Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comment> GetAll()
        {
            throw new NotImplementedException();
        }

        public Comment GetOne(int id)
        {
            throw new NotImplementedException();
        }

        public Comment GetOne(string name)
        {
            throw new NotImplementedException();
        }

        public Comment Update(Comment entity)
        {
            throw new NotImplementedException();
        }
    }
}

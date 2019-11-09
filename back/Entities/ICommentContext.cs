using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Entities
{
    public interface ICommentContext
    {
        Comment InsertComment(Comment comment);
    }
}

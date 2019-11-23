using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Tools
{
    public interface ILogger
    {
        void SetUp();
        void AddInfoLog(string message);
        void AddErrorLog(string message);
    }
}

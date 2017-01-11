using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Exceptions
{
    public class DataLoadException : Exception
    {
        public DataLoadException(string message) : base(message) { }
        public DataLoadException(string message, Exception inner) : base(message, inner) { }
    }
}

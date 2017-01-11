using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Exceptions
{
    public class DataSaveException : Exception
    {
        public DataSaveException(string message) : base(message) { }
        public DataSaveException(string message, Exception inner) : base(message, inner) { }
    }
}

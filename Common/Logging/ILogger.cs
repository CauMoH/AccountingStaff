using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logging
{
    public interface ILogger
    {
        void WriteError(Exception ex);
        void WriteError(string error, Exception ex);
        void WriteError(string error);
        void WriteInformation(string text);
    }
}

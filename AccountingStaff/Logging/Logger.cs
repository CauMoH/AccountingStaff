using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingStaff.Logging
{
    internal sealed class Logger : ILogger
    {
        public void WriteError(Exception ex)
        {
            LoggerFacade.WriteError(ex);
        }

        public void WriteError(string error, Exception ex)
        {
            LoggerFacade.WriteError(error, ex);
        }

        public void WriteError(string error)
        {
            LoggerFacade.WriteError(error);
        }

        public void WriteInformation(string text)
        {
            LoggerFacade.WriteInformation(text);
        }
    }
}

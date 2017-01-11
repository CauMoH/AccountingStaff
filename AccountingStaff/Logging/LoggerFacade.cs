using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingStaff.Logging
{
    internal static class LoggerFacade
    {
        private static readonly LogWriter _logWriter = new LogWriterFactory().Create();

        private const string CategoryError = "Error";
        private const string CategoryInformation = "Information";

        public static void WriteError(Exception ex)
        {
            WriteError(ex.ToString());
        }

        public static void WriteError(string error, Exception ex)
        {
            WriteError(string.Format("{0}{1}{2}", error, Environment.NewLine, ex.ToString()));
        }

        public static void WriteError(string error)
        {
            _logWriter.Write(error, CategoryError, 0, 0, System.Diagnostics.TraceEventType.Error);
        }

        public static void WriteInformation(string text)
        {
            _logWriter.Write(text, CategoryInformation, 0, 0, System.Diagnostics.TraceEventType.Information);
        }
    }
}

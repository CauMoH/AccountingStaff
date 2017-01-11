using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logging
{
    public sealed class NamedLogger
    {
        private readonly ILogger _logger;
        private readonly string _name;
        private readonly bool _enableDebugLog;

        public NamedLogger(ILogger logger, string name, bool enableDebugLog = false)
        {
            _logger = logger;
            _name = name;
            _enableDebugLog = enableDebugLog;
        }

        public void WriteInfo(string message)
        {
            WriteInternal(message, false, null);
        }

        public void WriteError(string message, Exception ex)
        {
            WriteInternal(message, true, ex);
        }

        public void WriteError(Exception ex)
        {
            WriteInternal(null, true, ex);
        }

        private void WriteInternal(string message, bool isError, Exception ex)
        {
            var msg = _name + ": " + message;
            if (isError)
            {
                if (message != null)
                {
                    _logger.WriteError(msg, ex);
                }
                else
                {
                    _logger.WriteError(ex);
                }
            }
            else
            {
                _logger.WriteInformation(msg);
            }

            if (_enableDebugLog)
            {
                Debug.WriteLine(msg);
            }
        }
    }
}

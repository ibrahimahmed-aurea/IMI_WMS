using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.Wms.Mobile.UI
{
    [Serializable]
    public class ServerFaultException : Exception
    {
        private string _type;
        private string _errorCode;
        private string _serverStackTrace;

        public ServerFaultException() { }
        public ServerFaultException(string message) : base(message) { }

        public ServerFaultException(string message, string type, string errorCode, string serverStackTrace)
            : this(message, type, errorCode, serverStackTrace, null)
        {
        }

        public ServerFaultException(string message, string type, string errorCode, string serverStackTrace, Exception innerException)
            : base(message, innerException)
        {
            _type = type;
            _errorCode = errorCode;
            _serverStackTrace = serverStackTrace;
        }
                
        public string Type
        {
            get

            {
                return _type;
            }
        }

        public string ErrorCode
        {
            get
            {
                return _errorCode;
            }
        }

        public string ServerStackTrace
        {
            get
            {
                return _serverStackTrace;
            }
        }
    }
}

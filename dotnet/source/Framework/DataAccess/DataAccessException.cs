using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.Framework.DataAccess
{

    [Serializable]
    public class DataAccessException : Exception
    {
        public string ErrorCode { get; set; }

        public DataAccessException() { }
        public DataAccessException(string message) : base(message) { }
        public DataAccessException(string message, Exception inner) : base(message, inner) { }

        public DataAccessException(string message, string errorCode, Exception inne)
            : base(message, inne)
        {
            ErrorCode = errorCode;
        }

        protected DataAccessException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

}

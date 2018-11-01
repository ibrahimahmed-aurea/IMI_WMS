using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.Framework.Messaging.Adapter.Warehouse
{
    
    [global::System.Serializable]
    public class AbortPollException : Exception
    {
        public AbortPollException() { }
        public AbortPollException(string message) : base(message) { }
        public AbortPollException(string message, Exception inner) : base(message, inner) { }
        protected AbortPollException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

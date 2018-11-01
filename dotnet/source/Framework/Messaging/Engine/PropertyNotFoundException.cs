using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.Framework.Messaging.Engine
{
    
    [global::System.Serializable]
    public class PropertyNotFoundException : MessageEngineException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public PropertyNotFoundException() { }
        public PropertyNotFoundException(string propertyName) : this(propertyName, null) { }
        public PropertyNotFoundException(string propertyName, Exception inner) : base(string.Format("The property: \"{0}\" was not found in the message context.", propertyName, inner)) { }
        protected PropertyNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

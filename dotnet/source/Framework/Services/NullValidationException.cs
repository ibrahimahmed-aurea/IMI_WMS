using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.Framework.Services
{
    [global::System.Serializable]
    public class NullValidationException : ValidationException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //
        private string message;

        public NullValidationException() 
        { 
        }
        public NullValidationException(string message, string propertyName) : this(message, propertyName, null) 
        { 
        }
        public NullValidationException(string message, string propertyName, Exception inner) : base(message, propertyName, inner) 
        {
            this.message = message;
        }

        public override string Message
        {
            get
            {
                if (string.IsNullOrEmpty(PropertyName))
                    return string.Format("One or more required parameters are empty.\n\nError: {0}", message);
                else
                    return string.Format("Parameter \"{0}\" can not be empty.\n\nError: {1}", PropertyName, message);
            }
        }

        protected NullValidationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.Framework.Services
{
    [global::System.Serializable]
    public class ValidationException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        private string message;

        public string PropertyName { get; set; }

        public ValidationException() 
        { 
        }
        
        public ValidationException(string message, string propertyName) : this(message, propertyName, null) 
        {
        }
        
        public ValidationException(string message, string propertyName, Exception inner) : base(message, inner) 
        {
            this.PropertyName = propertyName;
            this.message = message;
        }

        public override string Message
        {
            get
            {
                if (string.IsNullOrEmpty(PropertyName))
                    return string.Format("One or more parameters have an invalid value.\n\nError: {0}", message);
                else
                    return string.Format("Parameter \"{0}\" has an invalid value.\n\nError: {1}", PropertyName, message);
            }
        }

        protected ValidationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

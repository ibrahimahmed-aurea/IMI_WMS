using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Imi.SupplyChain.UX
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

        public string PropertyName { get; set; }
            
        public ValidationException() 
        { 
        }

        public ValidationException(string message, string propertyName)
            : this(message, propertyName, null)
        {
        }
                
        public ValidationException(string message, string propertyName, Exception inner)
            : base(message, inner) 
        {
            PropertyName = propertyName;
        }

        protected ValidationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

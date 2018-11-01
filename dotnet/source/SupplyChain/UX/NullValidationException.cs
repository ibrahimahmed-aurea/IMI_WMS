using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Imi.SupplyChain.UX
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
                            
        public NullValidationException() 
        { 
        }

        public NullValidationException(string message, string propertyName)
            : this(message, propertyName, null)
        {
        }
                
        public NullValidationException(string message, string propertyName, Exception inner)
            : base(message, propertyName, inner) 
        {
            PropertyName = propertyName;
        }

        protected NullValidationException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context) { }
    }
}

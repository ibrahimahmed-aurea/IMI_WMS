using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.Framework.Services
{
    [global::System.Serializable]
    public class BatchException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //
        private int? position;

        public BatchException() { }
        public BatchException(string message, int? position) : this(message, position, null) { }
        public BatchException(string message, int? position, Exception inner)
            : base(message, inner)
        {
            this.position = position;
        }

        protected BatchException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public int? Position
        {
            get
            {
                return position;
            }
        }
    }
}

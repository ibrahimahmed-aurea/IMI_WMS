using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.Framework.Services
{
    [global::System.Serializable]
    public class MessageException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //
        private string messageXml;

        public MessageException(string messageXml) : this(messageXml, null) { }

        public MessageException(string messageXml, Exception inner)
            : base(messageXml, inner)
        {
            this.messageXml = messageXml;
        }

        protected MessageException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public string MessageXML
        {
            get
            {
                return messageXml;
            }
        }
    }
}

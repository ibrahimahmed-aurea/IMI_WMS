using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;

namespace Imi.Wms.Mobile.Server
{
    
    [global::System.Serializable]
    public class SessionNotFoundException : MessageEngineException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="SessionNotFoundException"/> class.</para>
        /// </summary>
        public SessionNotFoundException() { }
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="SessionNotFoundException"/> class.</para>
        /// </summary>
        /// <param name="message">
        /// </param>
        public SessionNotFoundException(string message) : base(message) { }
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="SessionNotFoundException"/> class.</para>
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <param name="inner">
        /// </param>
        public SessionNotFoundException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="SessionNotFoundException"/> class.</para>
        /// </summary>
        /// <param name="info">
        /// </param>
        /// <param name="context">
        /// </param>
        protected SessionNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.Framework.Messaging.Engine
{
    
    /// <summary>
    /// Base class for exceptions thrown by the message engine.
    /// </summary>
    [global::System.Serializable]
    public class MessageEngineException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="MessageEngineException"/> class.</para>
        /// </summary>
        public MessageEngineException() { }
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="MessageEngineException"/> class.</para>
        /// </summary>
        /// <param name="message">
        /// The message that describes the error. 
        /// </param>
        public MessageEngineException(string message) : base(message) { }
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="MessageEngineException"/> class.</para>
        /// </summary>
        /// <param name="message">
        /// The message that describes the error. 
        /// </param>
        /// <param name="inner">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified. 
        /// </param>
        public MessageEngineException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="MessageEngineException"/> class.</para>
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
        /// </param>
        protected MessageEngineException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

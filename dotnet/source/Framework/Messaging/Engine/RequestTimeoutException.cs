using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.Framework.Messaging.Engine
{
    
    /// <summary>
    /// The exception that is thrown when a request message times out
    /// </summary>
    [global::System.Serializable]
    public class RequestTimeoutException : MessageEngineException
    {
        private MultiPartMessage request;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="RequestTimeoutException"/> class.</para>
        /// </summary>
        public RequestTimeoutException() { }


        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="RequestTimeoutException"/> class.</para>
        /// </summary>
        /// <param name="message">
        /// The error message string.
        /// </param>
        public RequestTimeoutException(string message) : base(message) { }


        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="RequestTimeoutException"/> class.</para>
        /// </summary>
        /// <param name="message">
        /// The error message string.
        /// </param>
        /// <param name="request">
        /// The request message.
        /// </param>
        public RequestTimeoutException(string message, MultiPartMessage request)
            : this(message, request, null)
        { 
        
        }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="RequestTimeoutException"/> class.</para>
        /// </summary>
        /// <param name="message">
        /// The error message string.
        /// </param>
        /// <param name="request">
        /// The request message.
        /// </param>
        /// <param name="inner">
        /// The inner exception reference. 
        /// </param>
        public RequestTimeoutException(string message, MultiPartMessage request, Exception inner) 
            : base(message, inner)
        {
            this.request = request;
        }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="RequestTimeoutException"/> class.</para>
        /// </summary>
        /// <param name="info">
        /// </param>
        /// <param name="context">
        /// </param>
        protected RequestTimeoutException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        /// <summary>
        /// Returns the request message.
        /// </summary>
        public MultiPartMessage RequestMessage
        {
            get
            {
                return request;
            }
        }
    }
    
}

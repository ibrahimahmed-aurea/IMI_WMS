using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Imi.Framework.Messaging.Engine;

namespace Imi.Framework.Messaging.Adapter
{
    /// <summary>
    /// The exception that is thrown by an <see cref="AdapterBase"/> class.
    /// </summary>
    [Serializable]
    public class AdapterException : MessageEngineException
    {
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="AdapterException"/> class.</para>
        /// </summary>
        public AdapterException() : base() { }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="AdapterException"/> class.</para>
        /// </summary>
        /// <param name="message">
        /// </param>
        public AdapterException(string message) : base(message) {}
        
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="AdapterException"/> class.</para>
        /// </summary>
        /// <param name="info">
        /// </param>
        /// <param name="context">
        /// </param>
        protected AdapterException(SerializationInfo info, StreamingContext context) : base(info, context) {}
        
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="AdapterException"/> class.</para>
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <param name="innerException">
        /// </param>
        public AdapterException(string message, Exception innerException) : base(message, innerException) { }
    }
}

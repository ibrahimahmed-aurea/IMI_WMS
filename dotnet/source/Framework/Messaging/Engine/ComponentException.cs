using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Imi.Framework.Messaging.Engine
{
    /// <summary>
    /// The exception that is thrown during invokation of a <see cref="PipelineComponent"/>.
    /// </summary>
    [Serializable]
    public class ComponentException : MessageEngineException
    {
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="ComponentException"/> class.</para>
        /// </summary>
        public ComponentException() : base() { }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="ComponentException"/> class.</para>
        /// </summary>
        /// <param name="message">
        /// </param>
        public ComponentException(string message) : base(message) {}

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="ComponentException"/> class.</para>
        /// </summary>
        /// <param name="info">
        /// </param>
        /// <param name="context">
        /// </param>
        protected ComponentException(SerializationInfo info, StreamingContext context) : base(info, context) {}

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="ComponentException"/> class.</para>
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <param name="innerException">
        /// </param>
        public ComponentException(string message, Exception innerException) : base(message, innerException) { }
    }
}

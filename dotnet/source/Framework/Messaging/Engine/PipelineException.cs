using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Imi.Framework.Messaging.Engine;

namespace Imi.Framework.Messaging.Engine
{
    /// <summary>
    /// The exception that is thrown by a <see cref="ComponentPipeline"/>.
    /// </summary>
    [Serializable]
    public class PipelineException : MessageEngineException
    {
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="PipelineException"/> class.</para>
        /// </summary>
        public PipelineException() : base() { }
        
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="PipelineException"/> class.</para>
        /// </summary>
        /// <param name="message">
        /// </param>
        public PipelineException(string message) : base(message) {}

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="PipelineException"/> class.</para>
        /// </summary>
        /// <param name="info">
        /// </param>
        /// <param name="context">
        /// </param>
        protected PipelineException(SerializationInfo info, StreamingContext context) : base(info, context) {}

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="PipelineException"/> class.</para>
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <param name="innerException">
        /// </param>
        public PipelineException(string message, Exception innerException) : base(message, innerException) { }
    }
}

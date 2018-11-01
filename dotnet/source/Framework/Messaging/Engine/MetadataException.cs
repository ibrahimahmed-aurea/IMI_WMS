using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Imi.Framework.Messaging.Engine;

namespace Imi.Framework.Messaging.Engine
{
    /// <summary>
    /// The exception that is thrown when there is an error in the Metadata of a <see cref="MultiPartMessage"/>.
    /// </summary>
    [Serializable]
    public class MetadataException : MessageEngineException
    {
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="MetadataException"/> class.</para>
        /// </summary>
        public MetadataException() : base() { }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="MetadataException"/> class.</para>
        /// </summary>
        /// <param name="message">
        /// </param>
        public MetadataException(string message) : base(message) {}

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="MetadataException"/> class.</para>
        /// </summary>
        /// <param name="info">
        /// </param>
        /// <param name="context">
        /// </param>
        protected MetadataException(SerializationInfo info, StreamingContext context) : base(info, context) {}

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="MetadataException"/> class.</para>
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <param name="innerException">
        /// </param>
        public MetadataException(string message, Exception innerException) : base(message, innerException) { }
    }
}

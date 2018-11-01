using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.Framework.Messaging.Engine
{
    /// <summary>
    /// Defines the persistence of a <see cref="PipelineComponent"/>.
    /// </summary>
    public enum PersistenceMode
    { 
        /// <summary>
        /// The component is persistent during the lifetime of the adapter.
        /// </summary>
        Adapter,
        /// <summary>
        /// The component is persistent during the lifetime of the endpoint.
        /// </summary>
        EndPoint
    }

    /// <summary>
    /// Defines the persistence a <see cref="PipelineComponent"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PersistenceAttribute : Attribute
    {
        private readonly PersistenceMode mode;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="PersistenceAttribute"/> class.</para>
        /// </summary>
        /// <param name="mode">
        /// The <see cref="PersistenceMode"/> of the <see cref="PipelineComponent"/>.
        /// </param>
        public PersistenceAttribute(PersistenceMode mode)
        {
            this.mode = mode;
        }

        /// <summary>
        /// Returns the <see cref="PersistenceMode"/> of the <see cref="PipelineComponent"/>.
        /// </summary>
        public PersistenceMode PersistenceMode
        {
            get 
            {
                return mode;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Imi.Framework.Messaging.Adapter;

namespace Imi.Framework.Messaging.Engine
{
    /// <summary>
    /// Base class for all pipeline components.
    /// </summary>
    public abstract class PipelineComponent
    {
        private readonly PropertyCollection configuration;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="PipelineComponent"/> class.</para>
        /// </summary>
        /// <param name="configuration">
        /// Configuration properties.
        /// </param>
        protected PipelineComponent(PropertyCollection configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Called by the pipeline to invoke the component.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <returns>A collection of messages produced by this component.</returns>
        public abstract Collection<MultiPartMessage> Invoke(MultiPartMessage msg);
        
        /// <summary>
        /// Checks if the component supports processing of a specified message.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <returns>True if the message is supported, othwerwise false.</returns>
        public abstract bool Supports(MultiPartMessage msg);

        /// <summary>
        /// Returns the configuration properties for this component.
        /// </summary>
        protected PropertyCollection Configuration
        {
            get
            {
                return configuration;
            }
        }
       
    }
}

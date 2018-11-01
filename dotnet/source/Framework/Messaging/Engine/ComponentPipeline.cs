using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Diagnostics;
using System.Threading;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter;
using Imi.Framework.Shared;

namespace Imi.Framework.Messaging.Engine
{
    /// <summary>
    /// The type of pipeline.
    /// </summary>
    public enum PipelineType
    { 
        /// <summary>
        /// The pipeline will be operating in Receive mode.
        /// </summary>
        Receive,
        /// <summary>
        /// The pipeline will be operating in Send mode.
        /// </summary>
        Send
    }

    [Persistence(PersistenceMode.Adapter)]
    internal sealed class DefaultPassThroughComponent : PipelineComponent
    {
        public DefaultPassThroughComponent(PropertyCollection configuration) 
            : base(configuration)
        { 
               
        }

        public override Collection<MultiPartMessage> Invoke(MultiPartMessage msg)
        {
            Collection<MultiPartMessage> resultCollection = new Collection<MultiPartMessage>();
            resultCollection.Add(msg);

            return resultCollection;
        }

        public override bool Supports(MultiPartMessage msg)
        {
            return true;
        }
      
    }
    
    /// <summary>
    /// Represents a pipeline in the message engine.
    /// </summary>
    public class ComponentPipeline
    {
        private PipelineType pipelineType;
        private List<ComponentInstanceManager> instanceManagerCollection;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="ComponentPipeline"/> class.</para>
        /// </summary>
        /// <param name="pipelineType">
        /// The type of pipeline (send/receive).
        /// </param>
        public ComponentPipeline(PipelineType pipelineType)
        {
            this.pipelineType = pipelineType;

            instanceManagerCollection = new List<ComponentInstanceManager>();

            AddComponent(typeof(DefaultPassThroughComponent));
        }

        internal Collection<MultiPartMessage> Execute(MultiPartMessage msg)
        {
            if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose)
                MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Executing component pipeline...");    
            
            int indentLevel = Trace.IndentLevel;

            Collection<MultiPartMessage> resultCollection = null;

            try
            {
                resultCollection = Execute(msg, 0);

                if (resultCollection != null)
                {
                    if ((resultCollection.Count > 1) && (pipelineType == PipelineType.Send))
                    {
                        throw new PipelineException("The send Pipeline returned more than one output message.");
                    }
                }
            }
            finally
            {
                Trace.IndentLevel = indentLevel;
            }
            
            return resultCollection;
        }

        private Collection<MultiPartMessage> Execute(MultiPartMessage msg, int componentIndex)
        {
            if (msg == null)
                throw new ArgumentNullException("msg");

            PipelineComponent component = instanceManagerCollection[componentIndex].GetInstance(msg);

            try
            {
                if (component.Supports(msg))
                {
                    if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose)
                        MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Invoking pipeline component: \"{0}\"...", component.GetType().Name);

                    Collection<MultiPartMessage> componentResultCollection = component.Invoke(msg);

                    if (componentResultCollection != null)
                    {
                        foreach (MultiPartMessage resultMsg in componentResultCollection)
                        {
                            CopyMetadata(msg, resultMsg);
                        }

                        componentIndex++;

                        if (componentIndex < instanceManagerCollection.Count)
                        {
                            Collection<MultiPartMessage> pipelineResultCollection = new Collection<MultiPartMessage>();

                            foreach (MultiPartMessage componentResultMsg in componentResultCollection)
                            {
                                Collection<MultiPartMessage> executeResultCollection = Execute(componentResultMsg, componentIndex);

                                if (executeResultCollection != null)
                                {
                                    foreach (MultiPartMessage pipelineResultMsg in executeResultCollection)
                                        pipelineResultCollection.Add(pipelineResultMsg);
                                }
                            }

                            if (pipelineResultCollection.Count > 0)
                                return pipelineResultCollection;
                            else
                                return null;
                        }

                        return componentResultCollection;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsCritical(ex))
                {
                    throw;
                }
                else
                {
                    try
                    {
                        if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Error) == SourceLevels.Error)
                            MessageEngine.Instance.Tracing.TraceData(TraceEventType.Error, 0, ex);

                        if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Error) == SourceLevels.Error)
                            MessageEngine.Instance.Tracing.TraceData(TraceEventType.Error, 0, string.Format("Message content:\n{0}", msg.ToXmlString()));
                    }
                    catch (Exception logEx)
                    {
                        if (ExceptionHelper.IsCritical(logEx))
                            throw;
                    }
                }
            }

            return null;
        }
        
        private static void CopyMetadata(MultiPartMessage sourceMsg, MultiPartMessage destMsg)
        {
            if (!destMsg.Metadata.Contains("ReceiveUri"))
                sourceMsg.Metadata.Copy("ReceiveUri", destMsg.Metadata);

            if (!destMsg.Metadata.Contains("ReceiveAdapterId"))
                sourceMsg.Metadata.Copy("ReceiveAdapterId", destMsg.Metadata);
                        
            if (!destMsg.Metadata.Contains("SendUri"))
                sourceMsg.Metadata.Copy("SendUri", destMsg.Metadata);

            if (!destMsg.Metadata.Contains("SendAdapterId"))
                sourceMsg.Metadata.Copy("SendAdapterId", destMsg.Metadata);
        }

        /// <summary>
        /// Returns the type of pipeline (send/receive).
        /// </summary>
        public PipelineType PipelineType
        {
            get
            {
                return pipelineType;
            }
        }

        /// <summary>
        /// Adds a component to the pipeline.
        /// </summary>
        /// <param name="componentType">The Type of the component to add.</param>
        /// <param name="configuration">Configuration properties.</param>
        public void AddComponent(Type componentType, PropertyCollection configuration)
        {
            ComponentInstanceManager instanceManager = new ComponentInstanceManager(componentType, configuration, pipelineType);

            instanceManagerCollection.Add(instanceManager);
        }

        /// <summary>
        /// Adds a component to the pipeline.
        /// </summary>
        /// <param name="componentType">The Type of the component to add.</param>
        public void AddComponent(Type componentType)
        {
            AddComponent(componentType, null);
        }
                
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using Imi.Framework.Messaging.Adapter;

namespace Imi.Framework.Messaging.Engine
{
    internal class ComponentInstanceManager
    {
        private Dictionary<object, PipelineComponent> instanceDictionary;
        private Type componentType;
        private PropertyCollection configuration;
        private ReaderWriterLock syncLock;
        private PersistenceMode persistenceMode;
        private PipelineType pipelineType;
        private EventHandler<AdapterEndPointEventArgs> endPointEventHandler;
        private ConstructorInfo componentConstructor;

        internal ComponentInstanceManager(Type componentType, PropertyCollection configuration, PipelineType pipelineType)
        {
            syncLock = new ReaderWriterLock();

            instanceDictionary = new Dictionary<object, PipelineComponent>();

            this.componentType = componentType;
            this.configuration = configuration;
            this.pipelineType = pipelineType;
            this.componentConstructor = componentType.GetConstructor(new Type[] { typeof(PropertyCollection) });

            if (configuration != null)
                configuration.Lock();
            
            persistenceMode = ((PersistenceAttribute)componentType.GetCustomAttributes(typeof(PersistenceAttribute), true)[0]).PersistenceMode;
        }

        internal PipelineComponent GetInstance(MultiPartMessage msg)
        {
            PipelineComponent component = null;
            object instanceKey = null;

            if (persistenceMode == PersistenceMode.Adapter)
            {
                if (pipelineType == PipelineType.Receive)
                    instanceKey = msg.Metadata.Read("ReceiveAdapterId");
                else
                    instanceKey = msg.Metadata.Read("SendAdapterId");
            }
            else if (persistenceMode == PersistenceMode.EndPoint)
            {
                if (pipelineType == PipelineType.Receive)
                    instanceKey = msg.Metadata.Read("ReceiveUri");
                else
                {
                    if (msg.Metadata.Contains("SendUri"))
                        instanceKey = msg.Metadata.Read("SendUri");
                    else
                        throw new PipelineException("Cannot create Component with EndPoint persistency when no EndPoint exists");
                }
            }

            syncLock.AcquireReaderLock(MessageEngine.Instance.LockMillisecondsTimeout);

            try
            {
                if (instanceDictionary.ContainsKey(instanceKey))
                    component = instanceDictionary[instanceKey];
            }
            finally
            {
                syncLock.ReleaseReaderLock();
            }

            if (component == null)
            {
                syncLock.AcquireWriterLock(MessageEngine.Instance.LockMillisecondsTimeout);
                                
                try
                {
                    if (instanceDictionary.ContainsKey(instanceKey))
                    {
                        if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose)
                            MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "{0}: Instance already created by another thread.", componentType.Name);
                        
                        component = instanceDictionary[instanceKey];
                    }
                    else
                    {
                        component = (PipelineComponent)componentConstructor.Invoke(new object[] {configuration});

                        instanceDictionary.Add(instanceKey, component);

                        if (persistenceMode == PersistenceMode.EndPoint)
                        { 
                            if (endPointEventHandler == null)
                            {
                                endPointEventHandler = new EventHandler<AdapterEndPointEventArgs>(EndPointDestroyedEventHandler);

                                AdapterBase adapter = MessageEngine.Instance.AdapterProxy.ResolveUriToAdapter((Uri)instanceKey);
                                adapter.EndPointDestroyed += endPointEventHandler;
                            }
                        }

                        if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose)
                            MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "{0}: Instance created for: \"{1}\".", componentType.Name, instanceKey);
                    }
                }
                finally
                {
                    syncLock.ReleaseWriterLock();
                }
            }
            
            return component;
        }
        
        private void EndPointDestroyedEventHandler(object sender, AdapterEndPointEventArgs e)
        {
            syncLock.AcquireWriterLock(MessageEngine.Instance.LockMillisecondsTimeout);

            try
            {
                instanceDictionary.Remove(e.EndPoint);
            }
            finally
            {
                syncLock.ReleaseWriterLock();
            }

            if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose)
                MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "{0}: Instance destroyed for EndPoint: \"{1}\".", componentType.Name, e.EndPoint.Uri);
        }
    }
}

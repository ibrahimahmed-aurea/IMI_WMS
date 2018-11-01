using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using System.Configuration;
using Imi.Framework.Messaging.Adapter;
using Imi.Framework.Messaging.Transform;
using Imi.Framework.Messaging.Configuration;
using Imi.Framework.Shared;

namespace Imi.Framework.Messaging.Engine
{
    /// <summary>
    /// The message engine class handles execution of component pipelines and subscribers.
    /// </summary>
    public sealed class MessageEngine : IDisposable
    {
        private class MessageEngineInstance
        {
            static MessageEngineInstance()
            {
                
            }

            internal static readonly MessageEngine instance = new MessageEngine();
        }
                        
        private AdapterProxy _adapterProxy;
        private Dictionary<AdapterBase, ComponentPipeline> _sendPipelineDictionary;
        private Dictionary<AdapterBase, ComponentPipeline> _receivePipelineDictionary;
        private List<AdapterBase> _adapterCollection;
        private SubscriptionManager _subscriptionManager;
        private Dictionary<string, CorrelationContext> _correlationDictionary;
        private ReaderWriterLock _correlationSyncLock;
        private TraceSource _traceSource;
        private ManualResetEvent _blockThreadsWaitEvent;
        private bool _isRunning;
        private bool _isDisposed;
                
        /// <summary>
        /// Timeout for deadlock detection. If a lock can't be acquired within the specified time an exception will be thrown.
        /// </summary>
        public readonly int LockMillisecondsTimeout;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="MessageEngine"/> class.</para>
        /// </summary>
        public MessageEngine()
        {
            _adapterProxy = new AdapterProxy();
            _sendPipelineDictionary = new Dictionary<AdapterBase, ComponentPipeline>();
            _receivePipelineDictionary = new Dictionary<AdapterBase, ComponentPipeline>();
            _subscriptionManager = new SubscriptionManager();
            _adapterCollection = new List<AdapterBase>();
            _correlationDictionary = new Dictionary<string, CorrelationContext>();
            _correlationSyncLock = new ReaderWriterLock();
            _traceSource = new TraceSource("MessageEngine");
                        
            _blockThreadsWaitEvent = new ManualResetEvent(false);

            LockMillisecondsTimeout = (ConfigurationManager.GetSection(MessagingSection.SectionKey) as MessagingSection).DeadlockTimeout;
        }

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~MessageEngine()
        {
            Dispose(false);
        }
                
        /// <summary>
        /// Returns the current message engine instance.
        /// </summary>
        public static MessageEngine Instance
        {
            get
            {
                return MessageEngineInstance.instance;   
            }
        }
        
        /// <summary>
        /// Binds a pipeline to an adapter.
        /// </summary>
        /// <param name="adapter">The <see cref="AdapterBase"/> to bind to.</param>
        /// <param name="pipeline">The <see cref="ComponentPipeline"/> to bind.</param>
        /// <remarks>
        /// When a pipeline has ben bound, all messages sent or received by the adapter will be 
        /// passed through the pipeline for processing.
        /// </remarks>
        public void Bind(AdapterBase adapter, ComponentPipeline pipeline)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(this.GetType().Name);

            if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Information) == SourceLevels.Information)
                MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Information, 0, "Binding adapter: \"{0}\".", adapter.AdapterId);

            if (adapter == null)
                throw new ArgumentNullException("adapter");
            else if (pipeline == null)
                throw new ArgumentNullException("pipeline");

            if (pipeline.PipelineType == PipelineType.Receive)
                _receivePipelineDictionary[adapter] = pipeline;
            else
                _sendPipelineDictionary[adapter] = pipeline;

            if (!_adapterCollection.Contains(adapter))
            {
                adapter.MessageReceived += new EventHandler<AdapterReceiveEventArgs>(MessageReceivedEventHandler);
                _adapterCollection.Add(adapter);
                _adapterProxy.RegisterAdapter(adapter);
            }
        }
        
        /// <summary>
        /// Returns the <see cref="SubscriptionManager"/> used to manage message subscriptions.
        /// </summary>
        public SubscriptionManager SubscriptionManager
        {
            get
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(this.GetType().Name);
                
                return _subscriptionManager;
            }
        }
                
        private void MessageReceivedEventHandler(object sender, AdapterReceiveEventArgs e)
        {
            if (!_isRunning)
            {
                return;
            }

            MultiPartMessage msg = e.Message;

            try
            {
                //Block incoming messages if startup in progress
                _blockThreadsWaitEvent.WaitOne();
                                
                //Create message metadata
                msg.Metadata.Write("ReceiveUri", e.ReceiveEndPoint.Uri);
                msg.Metadata.Write("ReceiveAdapterId", e.ReceiveEndPoint.Adapter.AdapterId);
                                                
                ComponentPipeline pipeline = _receivePipelineDictionary[e.ReceiveEndPoint.Adapter];
                Collection<MultiPartMessage> resultCollection = pipeline.Execute(msg);

                if (resultCollection != null)
                {
                    List<CorrelationContext> contextCollection = new List<CorrelationContext>();

                    foreach (MultiPartMessage resultMsg in resultCollection)
                    {
                        CorrelationContext context = CorrelateMessage(resultMsg);

                        if (context == null)
                        {
                            SubscriptionManager.DistributeMessage(resultMsg);
                        }
                        else
                        {
                            contextCollection.Add(context);
                        }
                    }
                                        
                    foreach (CorrelationContext context in contextCollection)
                    {
                        //Signal the requestor thread that a response has been received
                        context.CompleteRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsCritical(ex))
                {
                    throw;
                }
                else if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Error) == SourceLevels.Error && _isRunning)
                {
                    if (msg != null)
                        MessageEngine.Instance.Tracing.TraceData(TraceEventType.Error, msg.GetHashCode(), ex);
                    else
                        MessageEngine.Instance.Tracing.TraceData(TraceEventType.Error, 0, ex);
                }
            }
        }

        private CorrelationContext CorrelateMessage(MultiPartMessage msg)
        {
            _correlationSyncLock.AcquireReaderLock(LockMillisecondsTimeout);

            try
            {
                if (msg.Metadata.Contains("CorrelationId"))
                {
                    string correlationId = (string)msg.Metadata.Read("CorrelationId");

                    if (_correlationDictionary.ContainsKey(correlationId))
                    {
                        CorrelationContext context = _correlationDictionary[correlationId];
                        context.ResponseMessages.Add(msg);
                        return context;
                    }
                }
            }
            finally
            {
                _correlationSyncLock.ReleaseReaderLock();
            }

            return null;
            
        }

        /// <summary>
        /// Initializes adapters to start receiving/sending messages.
        /// </summary>
        public void Start()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(this.GetType().Name);

            if (_isRunning)
                throw new InvalidOperationException("Message Engine is already running.");

            if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Information) == SourceLevels.Information)
                MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Information, 0, "Starting Message Engine...");

            _isRunning = true;

            _blockThreadsWaitEvent.Reset();
                        
            foreach (AdapterBase adapter in _adapterCollection)
            {
                if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Information) == SourceLevels.Information)
                    MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Information, 0, "Initializing adapter: \"{0}\"...", adapter.AdapterId);    
                
                adapter.Initialize();
            }
                        
            if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Information) == SourceLevels.Information)
                MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Information, 0, "Startup complete.");    
            
            _blockThreadsWaitEvent.Set();
        }
                
        /// <summary>
        /// Stops the message engine.
        /// </summary>
        public void Stop()
        {
            Dispose();

            if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Information) == SourceLevels.Information)
                MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Information, 0, "Message Engine stopped.");
        }
        
        /// <summary>
        /// Transmits a message.
        /// </summary>
        /// <param name="msg">The message to transmit.</param>
        public void TransmitMessage(MultiPartMessage msg)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(this.GetType().Name);

            if (!_isRunning)
                throw new InvalidOperationException("Message Engine is not running.");
            
            CheckMetadata(msg);

            AdapterBase adapter = AdapterProxy.GetAdapterById(msg.Metadata.ReadAsString("SendAdapterId"));
                        
            ComponentPipeline pipeline = _sendPipelineDictionary[adapter];

            Collection<MultiPartMessage> resultCollection = pipeline.Execute(msg);
                        
            if (resultCollection != null)
            {
                //Check if message should be part of a transaction
                if (TransactionScope.Current != null)
                {
                    if (adapter is ITransactional)
                    {
                        ITransactional transactionalAdapter = adapter as ITransactional;

                        AdapterTransaction transaction = transactionalAdapter.StartTransaction(resultCollection[0]);
                        transaction.State = TransactionState.Started;

                        TransactionScope.Current.EnlistTransaction(transaction);
                        
                        transactionalAdapter.TransmitMessage(resultCollection[0], transaction);
                    }
                    else
                        throw new AdapterException("Adapter: \"" + adapter.AdapterId + "\" is not transactional.");
                }
                else
                    adapter.TransmitMessage(resultCollection[0]);
            }
                                    
        }
        
        /// <summary>
        /// Transmits a request and blocks until a response has been received, 
        /// or until the specified timeout has elapsed.
        /// </summary>
        /// <param name="msg">The request message to transmit.</param>
        /// <param name="correlationId">A string used to correlate the request with the response.</param>
        /// <param name="millisecondsTimeout">Timeout in milliseconds.</param>
        /// <param name="context">
        /// The <see cref="CorrelationContext"/> containing one or more response messages.
        /// </param>
        public void TransmitRequestMessage(MultiPartMessage msg, string correlationId, int millisecondsTimeout, out CorrelationContext context)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(this.GetType().Name);

            if (!_isRunning)
                throw new InvalidOperationException("Message Engine is not running.");
            
            if (correlationId == null)
                throw new ArgumentNullException("correlationId");
                        
            msg.Metadata.Write("CorrelationId", correlationId);

            context = new CorrelationContext();

            _correlationSyncLock.AcquireWriterLock(LockMillisecondsTimeout);

            try
            {
                _correlationDictionary.Add(correlationId, context);
            }
            finally
            {
                _correlationSyncLock.ReleaseWriterLock();
            }

            try
            {
                TransmitMessage(msg);

                if (!context.WaitForResponse(millisecondsTimeout))
                {
                    Uri sendUri = (Uri)msg.Metadata.Read("SendUri");
                    throw new RequestTimeoutException("Request timed out: \"" + sendUri + "\".", msg);
                }
            }
            finally
            {
                try
                {
                    context.Dispose();
                }
                finally
                {
                    _correlationSyncLock.AcquireWriterLock(LockMillisecondsTimeout);

                    try
                    {
                        _correlationDictionary.Remove(correlationId);
                    }
                    finally
                    {
                        _correlationSyncLock.ReleaseWriterLock();
                    }
                }
            }    
        }

        /// <summary>
        /// Transmits a request and blocks until a response has been received.
        /// </summary>
        /// <param name="msg">The request message to transmit.</param>
        /// <param name="correlationId">A string used to correlate the request with the response.</param>
        /// <param name="context">
        /// The <see cref="CorrelationContext"/> containing one or more response messages.
        /// </param>
        public void TransmitRequestMessage(MultiPartMessage msg, string correlationId, out CorrelationContext context)
        {
            TransmitRequestMessage(msg, correlationId, Timeout.Infinite, out context);
        }

        private void CheckMetadata(MultiPartMessage msg)
        {
            #region Loopback Metadata

            if (msg.Metadata.Contains("Loopback"))
            {
                if ((bool)msg.Metadata.Read("Loopback") == true)
                {
                    if (msg.Metadata.Contains("ReceiveUri"))
                        msg.Metadata.Write("SendUri", msg.Metadata.Read("ReceiveUri"));
                    else
                        throw new MetadataException("Metadata property: \"ReceiveUri\" is required for loopback.");
                }
            }

            #endregion

            Uri uri = null;

            if (msg.Metadata.Contains("SendUri"))
                uri = msg.Metadata.Read("SendUri") as Uri;
            else
                throw new MetadataException("Metadata property: \"SendUri\" is required for transmission.");

            if (!msg.Metadata.Contains("SendAdapterId"))
            {
                AdapterBase adapter = _adapterProxy.ResolveUriToAdapter(uri);

                if (adapter != null)
                {
                    msg.Metadata.Write("SendAdapterId", adapter.AdapterId);
                }
                else
                {
                    throw new MetadataException("Metadata property: \"SendAdapterId\" is required for transmission.");
                }
            }
        }

        /// <summary>
        /// Returns the <see cref="AdapterProxy"/> which manages Adapters and EndPoints.
        /// </summary>
        public AdapterProxy AdapterProxy
        {
            get
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(this.GetType().Name);

                return _adapterProxy;
            }
        }

        /// <summary>
        /// Returns true if the message engine is running.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
        }

        /// <summary>
        /// Returns the <see cref="TraceSource"/> of the message engine.
        /// </summary>
        public TraceSource Tracing
        {
            get
            {
                return _traceSource;
            }
        }


        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isRunning = false;
                
                Dispose(true);
	_isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    foreach (AdapterBase adapter in _adapterCollection)
                    {
                        adapter.Dispose();
                    }
                }
                finally
                {
                    _blockThreadsWaitEvent.Close();
                }
            }
        }
       
    }
    
}

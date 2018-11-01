using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Imi.Framework.Messaging.Engine;

namespace Imi.Framework.Messaging.Adapter
{
    /// <summary>
    /// The abstract base class which all adapters must inherit.
    /// </summary>
    public abstract class AdapterBase : IDisposable
    {
        public event EventHandler<AdapterReceiveEventArgs> MessageReceived;
        public event EventHandler<AdapterEndPointEventArgs> EndPointCreated;
        public event EventHandler<AdapterEndPointEventArgs> EndPointDestroyed;
        
        private string id;
        private PropertyCollection configuration;
        private List<AdapterEndPoint> endPointCollection;
        private Dictionary<string, CorrelationContext> correlationDictionary;
        private bool disposed;


        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="AdapterBase"/> class.</para>
        /// </summary>
        /// <param name="configuration">
        /// </param>
        /// <param name="adapterId">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 	<para>The argument <paramref name="configuration"/> is <langword name="null"/>.</para>
        /// 	<para>-or-</para>
        /// 	<para>The argument <paramref name="adapterId"/> is <langword name="null"/>.</para>
        /// </exception>
        protected AdapterBase(PropertyCollection configuration, string adapterId)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");
            
            if (adapterId == null)
                throw new ArgumentNullException("adapterId");

            this.configuration = configuration;
            this.id = adapterId;
            
            endPointCollection = new List<AdapterEndPoint>();
            correlationDictionary = new Dictionary<string, CorrelationContext>();
        }

        /// <summary>
        /// Adapter finalizer
        /// </summary>
        ~AdapterBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Called when a message is received by the adapter.
        /// </summary>
        /// <param name="msg">The message received.</param>
        /// <param name="endPoint">The <see cref="AdapterEndPoint"/> which the message was received from.</param>
        protected virtual void OnMessageReceived(MultiPartMessage msg, AdapterEndPoint endPoint)
        {
            try
            {
                //Safely invoke the event handler
                EventHandler<AdapterReceiveEventArgs> temp = MessageReceived;
                
                if (temp != null)
                    temp(this, new AdapterReceiveEventArgs(msg, endPoint));
            }
            catch (MessageEngineException ex)
            {
                if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Error) == SourceLevels.Error)
                    MessageEngine.Instance.Tracing.TraceData(TraceEventType.Error, 0, ex);
            }
        }

        /// <summary>
        /// Called when an <see cref="AdapterEndPoint"/> has been created by the adapter.
        /// </summary>
        /// <param name="endPoint">A reference to the created endpoint.</param>
        protected virtual void OnEndPointCreated(AdapterEndPoint endPoint)
        {
            try
            {
                lock (endPointCollection)
                    endPointCollection.Add(endPoint);

                //Safely invoke the event handler
                EventHandler<AdapterEndPointEventArgs> temp = EndPointCreated;

                if (temp != null)
                    temp(this, new AdapterEndPointEventArgs(endPoint));
            }
            catch (MessageEngineException ex)
            {
                if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Error) == SourceLevels.Error)
                    MessageEngine.Instance.Tracing.TraceData(TraceEventType.Error, 0, ex);
            }
        }
        
        /// <summary>
        /// Called when an <see cref="AdapterEndPoint"/> has been destroyed.
        /// </summary>
        /// <param name="endPoint">A reference to the destroyed endpoint.</param>
        protected virtual void OnEndPointDestroyed(AdapterEndPoint endPoint)
        {
            try
            {
                bool removed = false;

                lock (endPointCollection)
                    removed = endPointCollection.Remove(endPoint);

                if (removed)
                {
                    //Safely invoke the event handler
                    EventHandler<AdapterEndPointEventArgs> temp = EndPointDestroyed;

                    if (temp != null)
                        temp(this, new AdapterEndPointEventArgs(endPoint));
                }
            }
            catch (MessageEngineException ex)
            {
                if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Error) == SourceLevels.Error)
                    MessageEngine.Instance.Tracing.TraceData(TraceEventType.Error, 0, ex);
            }
        }

        /// <summary>
        /// Transmits a message over the adapter protocol.
        /// </summary>
        /// <param name="msg">The message to transmit.</param>
        public abstract void TransmitMessage(MultiPartMessage msg);

        /// <summary>
        /// Initializes the adapter for sending and receiving of messages.
        /// </summary>
        protected internal abstract void Initialize();
        
        /// <summary>
        /// Returns an array of endpoints associated with this adapter.
        /// </summary>
        public AdapterEndPoint[] GetEndPoints()
        {
                
            AdapterEndPoint[] endPointArray;

            lock (endPointCollection)
            {
                endPointArray = endPointCollection.ToArray();
            }

            return endPointArray;
            
        }

        /// <summary>
        /// Returns the protocol used by this adapter
        /// </summary>
        public abstract string ProtocolType
        {
            get;
        }

        /// <summary>
        /// Returns the id of this adapter
        /// </summary>
        public virtual string AdapterId
        {
            get
            {
                return id;
            }
        }

        /// <summary>
        /// Returns the configuration of this adapter.
        /// </summary>
        protected virtual PropertyCollection Configuration
        {
            get
            {
                return configuration;
            }
        }

        protected bool IsDisposed
        {
            get
            {
                return disposed;
            }
        }
                
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes any unmanaged resources used by this adapter.
        /// </summary>
        /// <param name="disposing">True if called from user code.</param>
        protected virtual void Dispose(bool disposing)
        { 
            disposed = true;
        }
        
        protected void HandleAsyncException(Exception ex)
        {
            if (MessageEngine.Instance.IsRunning)
            {
                AdapterException wrappedException
                    = new AdapterException("An exception was thrown during an asynchronous operation in adapter: \"" + AdapterId + "\"", ex);

                if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Error) == SourceLevels.Error)
                    MessageEngine.Instance.Tracing.TraceData(TraceEventType.Error, 0, wrappedException);
            }
        }
                
    }
}

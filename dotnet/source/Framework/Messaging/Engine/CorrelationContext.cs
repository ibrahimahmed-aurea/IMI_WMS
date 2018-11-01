using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using Imi.Framework.Messaging.Adapter;
using Imi.Framework.Messaging.Engine;

namespace Imi.Framework.Messaging.Engine
{
    /// <summary>
    /// Context used to correlate a response message with a request.
    /// </summary>
    public class CorrelationContext : IDisposable
    {
        private ManualResetEvent waitEvent;
        private Collection<MultiPartMessage> responseMessageCollection;
        private bool disposed;

        internal CorrelationContext()
        {
            waitEvent = new ManualResetEvent(false);
            responseMessageCollection = new Collection<MultiPartMessage>();
        }

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~CorrelationContext()
        {
            Dispose(false);
        }

        internal void CompleteRequest()
        {
            if (disposed)
                throw new ObjectDisposedException(this.GetType().Name);

            waitEvent.Set();
        }
        
        internal bool WaitForResponse(int millisecondsTimeout)
        {
            if (disposed)
                throw new ObjectDisposedException(this.GetType().Name);

            return waitEvent.WaitOne(millisecondsTimeout, false);
        }

        internal void WaitForResponse()
        {
            if (disposed)
                throw new ObjectDisposedException(this.GetType().Name);

            waitEvent.WaitOne();
        }

        /// <summary>
        /// Returns a collection of response messages correlated with the request.
        /// </summary>
        public Collection<MultiPartMessage> ResponseMessages
        {
            get
            {
                return responseMessageCollection;
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (!disposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                disposed = true;
                waitEvent.Close();
            }
        }
    }
}

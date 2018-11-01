using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using Imi.Framework.Messaging.Adapter;

namespace Imi.Framework.Messaging.Engine
{
    /// <summary>
    /// Any messages sent within this block will automatically enlist in a transaction.
    /// </summary>
    public class TransactionScope : IDisposable
    {
        private Dictionary<string, AdapterTransaction> transactionDictionary;
        private ReaderWriterLock syncLock;
        private bool disposed;
                
        [ThreadStatic]
        private static TransactionScope currentScope;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="TransactionScope"/> class.</para>
        /// </summary>
        public TransactionScope()
        {
            if (currentScope != null)
                throw new InvalidOperationException("A transaction scope is already defined for the current execution context.");

            transactionDictionary = new Dictionary<string, AdapterTransaction>();
            syncLock = new ReaderWriterLock();
            currentScope = this;
        
        }

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~TransactionScope()
        {
            Dispose(false);
        }
                
        internal AdapterTransaction this[string transactionId]
        {
            get
            {
                return GetTransactionById(transactionId);
            }
        }

        internal AdapterTransaction GetTransactionById(string transactionId)
        {
            AdapterTransaction transaction = null;

            syncLock.AcquireReaderLock(MessageEngine.Instance.LockMillisecondsTimeout);

            try
            {
                transactionDictionary.TryGetValue(transactionId, out transaction);
            }
            finally
            {
                syncLock.ReleaseReaderLock();
            }

            return transaction;
        }
        
        /// <summary>
        /// Completes the transaction scope by committing all enlisted transactions.
        /// </summary>
        public void Complete()
        {
            if (disposed)
                throw new ObjectDisposedException(this.GetType().Name);

            lock (transactionDictionary)
            {
                foreach (string transactionId in transactionDictionary.Keys)
                {
                    try
                    {
                        AdapterTransaction transaction = transactionDictionary[transactionId];

                        if (transaction != null)
                        {
                            ((ITransactional)transaction.Adapter).Commit(transaction);
                            transaction.State = TransactionState.Commited;
                        }
                    }
                    catch (MessageEngineException ex)
                    {
                        if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Error) == SourceLevels.Error)
                            MessageEngine.Instance.Tracing.TraceData(TraceEventType.Error, 0, ex);
                    }
                }
            }
        }

        /// <summary>
        /// Aborts all transactions enlisted in the current scope.
        /// </summary>
        private void Abort()
        {
            lock (transactionDictionary)
            {
                foreach (string transactionId in transactionDictionary.Keys)
                {
                    try
                    {
                        AdapterTransaction transaction = transactionDictionary[transactionId];

                        if ((transaction != null) && (transaction.State == TransactionState.Started))
                        {
                            ((ITransactional)transaction.Adapter).Abort(transaction);
                            transaction.State = TransactionState.Aborted;
                        }
                    }
                    catch (MessageEngineException ex)
                    {
                        if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Error) == SourceLevels.Error)
                            MessageEngine.Instance.Tracing.TraceData(TraceEventType.Error, 0, ex);
                    }
                }
            }
        }

        internal void EnlistTransaction(AdapterTransaction transaction)
        {
            lock (transactionDictionary)
            {
                transactionDictionary[transaction.TransactionId] = transaction;
            }
        }

        /// <summary>
        /// Gets the transaction scope for the current execution context.
        /// </summary>
        /// <remarks>
        /// If no scope is defined, null is returned.
        /// </remarks>
        public static TransactionScope Current
        {
            get
            {
                return currentScope;
            }
        }
        
        #region IDisposable Members

        /// <summary>
        /// Releases all resources used by the TransactionScope.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }
                
        #endregion

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    Abort();
                }
                finally
                {
                    currentScope = null;
                }
            }
        
        }

    }
}

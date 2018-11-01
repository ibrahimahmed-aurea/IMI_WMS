using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Transaction;
using Spring.Transaction.Support;
using Imi.Framework.DataAccess;

namespace Imi.SupplyChain.Settings.DataAccess.NHibernate
{
    public class HibernateTransactionScope : ITransactionScope
    {
        private bool disposed;
        private bool isNested;
        private bool isCompleted;

        private bool IsAborted { get; set; }

        private IPlatformTransactionManager transactionManager;
        private ITransactionStatus status;

        [ThreadStatic]
        private static HibernateTransactionScope currentScope;

        public HibernateTransactionScope(IPlatformTransactionManager transactionManager)
        {
            if (currentScope != null)
                isNested = true;
            else
            {
                this.transactionManager = transactionManager;
                this.Start();
                currentScope = this;
            }
        }

        ~HibernateTransactionScope()
        {
            Dispose(false);
        }


        private void Start()
        {
            DefaultTransactionDefinition def = new DefaultTransactionDefinition();
            def.PropagationBehavior = TransactionPropagation.Required;
            status = transactionManager.GetTransaction(def);
        }

        public void Complete()
        {
            if (disposed)
                throw new ObjectDisposedException(this.GetType().Name);

            if (!isNested)
            {
                if (IsAborted)
                    throw new InvalidOperationException("The transaction has been aborted by a nested scope.");

                if (transactionManager != null)
                    transactionManager.Commit(status);
            }

            isCompleted = true;
        }

        private void Abort()
        {
            if (isNested)
            {
                HibernateTransactionScope.Current.IsAborted = true;
                return;
            }

            if (transactionManager != null)
            {
                if (!status.Completed)
                {
                    transactionManager.Rollback(status);
                }
            }
        }

        public static HibernateTransactionScope Current
        {
            get
            {
                return currentScope;
            }
        }

        #region IDisposable Members

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
                    if (!isCompleted)
                        Abort();
                }
                finally
                {
                    if (!isNested)
                        currentScope = null;
                }
            }

        }

    }
}

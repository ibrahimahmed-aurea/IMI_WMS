using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Data;

namespace Imi.Framework.DataAccess
{
    public class TransactionScope : ITransactionScope
    {
        private bool disposed;
        private bool isNested;
        private bool isCompleted;

        private bool IsAborted { get; set; }

        [ThreadStatic]
        private static TransactionScope currentScope;

        public TransactionScope()
        {
            if (currentScope != null)
                isNested = true;                
            else                        
                currentScope = this;
        }
        
        ~TransactionScope()
        {
            Dispose(false);
        }
                        
        internal IDbTransaction Transaction { get; set; }
                
        public void Complete()
        {
            if (disposed)
                throw new ObjectDisposedException(this.GetType().Name);

            if (!isNested)
            {


                if (IsAborted)
                    throw new InvalidOperationException("The transaction has been aborted by a nested scope.");

                try
                {
                    if (Transaction != null)
                        Transaction.Commit();
                }
                finally
                {
                    if ((Transaction != null) && (Transaction.Connection != null))
                        Transaction.Connection.Close();
                }
            }

            isCompleted = true;
        }

        private void Abort()
        {
            if (isNested)
            {
                TransactionScope.Current.IsAborted = true;
                return;
            }
            
            try
            {
                if (Transaction != null)
                    Transaction.Rollback();
            }
            finally
            {
                if ((Transaction != null) && (Transaction.Connection != null))
                    Transaction.Connection.Close();
            }
        }
                
        public static TransactionScope Current
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

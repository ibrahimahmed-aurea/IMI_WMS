using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Adapter;
using System.Threading;

namespace Imi.Framework.Messaging.Engine
{
    public enum TransactionState
    { 
        Unknown,
        Started,
        Commited,
        Aborted,
    }

    /// <summary>
    /// Transaction wrapper class.
    /// </summary>
    public sealed class AdapterTransaction
    {
        private readonly object underlyingTransaction;
        private readonly AdapterBase adapter;
        private readonly AdapterEndPoint endPoint;
        private readonly string id;
        private TransactionState state;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="AdapterTransaction"/> class.</para>
        /// </summary>
        /// <param name="id">
        /// The id string of the transaction.
        /// </param>
        /// <param name="underlyingTransaction">
        /// The underlying transaction object.
        /// </param>
        /// <param name="endPoint">
        /// The endpoint over which the transaction spans.
        /// </param>
        public AdapterTransaction(string id, object underlyingTransaction, AdapterEndPoint endPoint)
            : this(id, underlyingTransaction, endPoint.Adapter)
        {
            this.endPoint = endPoint;
        }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="AdapterTransaction"/> class.</para>
        /// </summary>
        /// <param name="id">
        /// The id string of the transaction.
        /// </param>
        /// <param name="underlyingTransaction">
        /// The underlying transaction object.
        /// </param>
        /// <param name="adapter">
        /// The adapter owning the transaction.
        /// </param>
        public AdapterTransaction(string id, object underlyingTransaction, AdapterBase adapter)
        {
            this.id = id;
            this.underlyingTransaction = underlyingTransaction;
            this.adapter = adapter;
            this.state = TransactionState.Unknown;
        }

        /// <summary>
        /// Returns the underlying transaction object.
        /// </summary>
        public object UnderlyingTransaction
        {
            get
            {
                return underlyingTransaction;
            }
        }

        /// <summary>
        /// Returns the adapter owning the transaction.
        /// </summary>
        public AdapterBase Adapter
        {
            get
            {
                return adapter;
            }
        }

        /// <summary>
        /// Returns the endpoint over which the transaction spans.
        /// </summary>
        public AdapterEndPoint EndPoint
        {
            get
            {
                return endPoint;
            }
        }

        /// <summary>
        /// Returns the Id string of the transaction.
        /// </summary>
        public string TransactionId
        {
            get
            {
                return id;
            }
        }

        public TransactionState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }
    }
}

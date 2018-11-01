using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;

namespace Imi.Framework.Messaging.Adapter
{
    /// <summary>
    /// Specifies that the adapter supports transactions.
    /// </summary>
    public interface ITransactional
    {
        /// <summary>
        /// Callback method to commit a transaction that was started by this adapter.
        /// </summary>
        /// <param name="transaction">The transaction to commit.</param>
        void Commit(AdapterTransaction transaction);
        
        /// <summary>
        /// Callback method to abort a transaction that was started by this adapter.
        /// </summary>
        /// <param name="transaction">The transaction to abort.</param>
        void Abort(AdapterTransaction transaction);
        
        /// <summary>
        /// Transmits a message as part of a transaction.
        /// </summary>
        /// <param name="msg">The message to transmit.</param>
        /// <param name="transaction">The transaction in which the message is enlisted.</param>
        void TransmitMessage(MultiPartMessage msg, AdapterTransaction transaction);
        
        /// <summary>
        /// Starts a transaction for this adapter.
        /// </summary>
        /// <param name="msg">The message that is to be enlisted in the transaction.</param>
        /// <returns>The transaction in which the message should be enlisted.</returns>
        /// <remarks>
        /// It's up to the adapter to decide if it should return a an existing transaction or start a new one.
        /// Any current transactions can be obtained from the <see cref="TransactionScope"/> class.
        /// </remarks>
        AdapterTransaction StartTransaction(MultiPartMessage msg);
    }
}

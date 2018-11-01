#define ODP_NET
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

#if ODP_NET
    using Oracle.DataAccess.Client;
#else
    using System.Data.OracleClient;
#endif

using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Threading;
using Imi.Framework.Messaging.Adapter;
using Imi.Framework.Messaging.Engine;

namespace Imi.Framework.Messaging.Adapter.Warehouse
{
        
    /// <summary>
    /// Adapter for IMI Warehouse.
    /// </summary>
    public class WarehouseAdapter : AdapterBase, ITransactional
    {
        private class DbCommandWrapper : IDisposable
        {
            private IDbCommand command;
            private bool isNonQuery;
            private string instanceId;
            private bool disposed;

            public DbCommandWrapper(IDbCommand command, string instanceId, bool isNonQuery)
            {
                this.command = command;
                this.isNonQuery = isNonQuery;
                this.instanceId = instanceId;
            }

            ~DbCommandWrapper()
            {
                Dispose(false);
            }

            public IDbCommand Command
            {
                get
                {
                    return command;
                }
            }

            public bool IsNonQuery
            {
                get
                {
                    return isNonQuery;
                }
            }

            public string InstanceId
            {
                get 
                {
                    return instanceId; 
                }
            }
                        
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected void Dispose(bool disposing)
            {
                if (!disposed)
                {
                    if (disposing)
                    {
                        if (command != null)
                        {
                            command.Dispose();
                        }
                    }

                    disposed = true;
                }
            }
        }

        private List<Timer> pollTimerCollection;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="WarehouseAdapter"/> class.</para>
        /// </summary>
        /// <param name="configuration">
        /// Configuration properties.
        /// </param>
        /// <param name="adapterId">
        /// The Id string of the adapter.
        /// </param>
        public WarehouseAdapter(Imi.Framework.Messaging.Engine.PropertyCollection configuration, string adapterId) 
            : base(configuration, adapterId)
        {
            pollTimerCollection = new List<Timer>();
        }
                
        private MultiPartMessage ExecuteCommand(DbCommandWrapper wrapper, string messageType)
        {
            IDataReader reader = null;
            MultiPartMessage msg = new MultiPartMessage(messageType);

            try
            {
                DateTime startTime = DateTime.Now;

                wrapper.Command.Prepare();

                if (!wrapper.IsNonQuery)
                {
                    reader = wrapper.Command.ExecuteReader();

                    while (reader.Read())
                    {
                        MessagePart part = new MessagePart();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            part.Properties.Write(reader.GetName(i), reader.GetValue(i));
                        }

                        msg.Parts.Add(part);
                    }
                }
                else
                {
                    wrapper.Command.ExecuteNonQuery();
                }

                //Promote output parameters to message properties
                foreach (IDbDataParameter param in wrapper.Command.Parameters)
                {
                    if ((param.Direction == ParameterDirection.Output) || (param.Direction == ParameterDirection.InputOutput))
                    {
                        msg.Properties.Write(param.ParameterName, param.Value);
                    }
                }

                TimeSpan creationTime = DateTime.Now - startTime;

                if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose)
                    MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Message created in: {0}s {1}ms", creationTime.Seconds, creationTime.Milliseconds);
            }
            catch (OracleException ex)
            {
                throw new AdapterException("Failed to transmit message to Warehouse instance: \"" + wrapper.InstanceId + "\".", ex);
            }
            catch (DbException ex)
            {
                throw new AdapterException("Failed to transmit message to Warehouse instance: \"" + wrapper.InstanceId + "\".", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new AdapterException("Failed to transmit message to Warehouse instance: \"" + wrapper.InstanceId + "\".", ex);
            }
            finally
            {
                try
                {
                    if (reader != null)
                        reader.Close();
                }
                finally
                {
                    if (reader != null)
                        reader.Dispose();
                }
            }
                        
            return msg;
        }
          
        private DbCommandWrapper CreateDbCommand(MultiPartMessage msg)
        {
            bool isNonQuery = true;
                        
            IDbCommand command = new OracleCommand();
            command.CommandType = CommandType.StoredProcedure;
            
            #if ODP_NET
            ((OracleCommand)command).BindByName = true;
            #endif

            Uri uri = msg.Metadata.Read("SendUri") as Uri;
            string instanceId = uri.Host;
            command.CommandText = uri.Segments[1] + uri.Segments[2];
            command.CommandText = command.CommandText.Replace("/", ".");
            
            foreach (string propertyName in msg.Properties)
            {
                IDbDataParameter param = command.CreateParameter();
                param.ParameterName = propertyName;

                Type paramType = msg.Properties.Read(propertyName).GetType();
                                
                if (paramType == typeof(string))
                {
                    param.DbType = DbType.String;
                }
                else if (paramType == typeof(short))
                {
                    param.DbType = DbType.Decimal;
                }
                else if (paramType == typeof(int))
                {
                    param.DbType = DbType.Decimal;
                }
                else if (paramType == typeof(double))
                {
                    param.DbType = DbType.Decimal;
                }
                else if (paramType == typeof(DateTime))
                {
                    param.DbType = DbType.DateTime;
                }
                
                                
                if (propertyName.EndsWith("_I"))
                {
                    param.Direction = ParameterDirection.Input;
                    param.Value = msg.Properties.Read(propertyName);
                }
                else if (propertyName.EndsWith("Cur_O"))
                {
                    isNonQuery = false;
                    param.Direction = ParameterDirection.Output;
                    
                    #if ODP_NET
                        ((OracleParameter)param).OracleDbType = OracleDbType.RefCursor;
                    #else
                        ((OracleParameter)param).OracleType = OracleType.Cursor;
                    #endif
                }
                else if (propertyName.EndsWith("_O"))
                {
                    param.Direction = ParameterDirection.Output;

                    if (param.DbType == DbType.String)
                    {
                        if (msg.Metadata.Contains(propertyName))
                            param.Size = msg.Metadata.ReadAsInt(propertyName);
                        else
                            param.Size = 255;
                    }
                }
                else if (propertyName.EndsWith("_IO"))
                {
                    param.Direction = ParameterDirection.InputOutput;

                    if (param.DbType == DbType.String)
                        param.Size = 255;

                    param.Value = msg.Properties.Read(propertyName);
                }
                                
                command.Parameters.Add(param);
            }
                        
            return new DbCommandWrapper(command, instanceId, isNonQuery);
        }

        private WarehouseAdapterEndPoint CreateEndPoint(string instanceId)
        {
            try
            {
                IDbConnection connection = new OracleConnection(Configuration.ReadAsString(instanceId));

                connection.Open();
                
                WarehouseAdapterEndPoint endPoint = new WarehouseAdapterEndPoint(this, instanceId, connection);

                OnEndPointCreated(endPoint);

                return endPoint;
            }
            catch (InvalidOperationException ex)
            {
                throw new AdapterException("No connection could be made to Warehouse instance: \"" + instanceId + "\".", ex);
            }
            catch (OracleException ex)
            {
                throw new AdapterException("No connection could be made to Warehouse instance: \"" + instanceId + "\".", ex);
            }
            catch (DbException ex)
            {
                throw new AdapterException("No connection could be made to Warehouse instance: \"" + instanceId + "\".", ex);
            }
            
        }

        private AdapterTransaction CreateTransaction(string instanceId)
        {
            try
            {
                WarehouseAdapterEndPoint endPoint = CreateEndPoint(instanceId);
                IDbTransaction transaction = endPoint.Connection.BeginTransaction(IsolationLevel.ReadCommitted);

                AdapterTransaction adapterTransaction = new AdapterTransaction(instanceId, transaction, endPoint);
                adapterTransaction.State = TransactionState.Started;

                return adapterTransaction;
            }
            catch (InvalidOperationException ex)
            {
                throw new AdapterException("Failed to start transaction for Warehouse instance: \"" + instanceId + "\".", ex);
            }
            catch (OracleException ex)
            {
                throw new AdapterException("Failed to start transaction for Warehouse instance: \"" + instanceId + "\".", ex);
            }
            catch (DbException ex)
            {
                throw new AdapterException("Failed to start transaction for Warehouse instance: \"" + instanceId + "\".", ex);
            }
        }


        /// <summary>
        /// Initializes the adapter for sending and receiving of messages.
        /// </summary>
        protected internal override void Initialize()
        {
            foreach (string propertyName in Configuration)
            {
                if (Configuration[propertyName] is MultiPartMessage)
                {
                    MultiPartMessage msg = Configuration[propertyName] as MultiPartMessage;

                    if (!msg.Metadata.Contains("PollPeriod"))
                        throw new MetadataException("Unable to setup polling, missing metadata property: \"PollPeriod\".");

                    msg.Metadata.Write("Polling", false);

                    Timer pollTimer = new Timer(ExecutePollCallback, msg, 0, msg.Metadata.ReadAsInt("PollPeriod"));
                    pollTimerCollection.Add(pollTimer);
                }
            }
        }

        private void ExecutePollCallback(object state)
        { 
            MultiPartMessage msg = state as MultiPartMessage;
            
            lock (msg)
            {
                if ((bool)msg.Metadata.Read("Polling"))
                    return;
                
                msg.Metadata.Write("Polling", true);
            }

            try
            {
                while (MessageEngine.Instance.IsRunning)
                {
                    try
                    {
                        this.TransmitMessage(msg);
                    }
                    catch (AbortPollException)
                    {
                        break;
                    }
                }
            }
            catch (MessageEngineException ex)
            {
                HandleAsyncException(ex);
            }
            finally
            {
                lock (msg)
                {
                    msg.Metadata.Write("Polling", false);
                }
            }
        }

        /// <summary>
        /// Returns the protocol used by this adapter
        /// </summary>
        public override string ProtocolType
        {
            get
            {
                return "warehouse";
            }
        }
                
        private string GetAlarmText(string almId, string nLangCod, IDbConnection connection, IDbTransaction transaction)
        {
            if (string.IsNullOrEmpty(nLangCod))
                nLangCod = "ENU";
            
            IDbCommand command = connection.CreateCommand();
            
            #if ODP_NET
            ((OracleCommand)command).BindByName = true;
            #endif
 
            command.Transaction = transaction;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "wlsystem.getalmtxt";

            IDbDataParameter p1 = command.CreateParameter();
            p1.ParameterName = "ALMID_I";
            p1.DbType = DbType.String;
            p1.Direction = ParameterDirection.Input;
            p1.Value = almId;
            
            IDbDataParameter p2 = command.CreateParameter();
            p2.ParameterName = "NLANGCOD_I";
            p2.DbType = DbType.String;
            p2.Direction = ParameterDirection.Input;
            p2.Value = nLangCod;

            IDbDataParameter p3 = command.CreateParameter();
            p3.ParameterName = "ALMTXT_O";
            p3.Size = 400;
            p3.DbType = DbType.String;
            p3.Direction = ParameterDirection.Output;

            command.Parameters.Add(p1);
            command.Parameters.Add(p2);
            command.Parameters.Add(p3);

            try
            {
                command.Prepare();

                command.ExecuteNonQuery();

                return ((IDbDataParameter)command.Parameters["ALMTXT_O"]).Value.ToString();
            }
            catch (InvalidOperationException)
            {
                return almId;
            }
            catch (OracleException)
            {
                return almId;
            }
            catch (DbException)
            {
                return almId;
            }

        }

        /// <summary>
        /// Transmits a message over the adapter protocol
        /// </summary>
        /// <param name="msg">The message to transmit</param>
        public override void TransmitMessage(MultiPartMessage msg)
        {
            if (msg == null)
                throw new ArgumentNullException("msg");
            
            TransmitMessage(msg, null);
            
        }

        /// <summary>
        /// Callback method to commit a transaction that was started by this adapter.
        /// </summary>
        /// <param name="transaction">The transaction to commit.</param>
        public void Commit(AdapterTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            IDbTransaction dbTransaction = transaction.UnderlyingTransaction as IDbTransaction;

            try
            {
                //Commit transaction
                dbTransaction.Commit();
                transaction.State = TransactionState.Commited;
                
            }
            catch (OracleException ex)
            {
                throw new AdapterException("The transaction could not be committed.", ex);
            }
            catch (DbException ex)
            {
                throw new AdapterException("The transaction could not be committed.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new AdapterException("The transaction could not be committed.", ex);
            }
            finally
            {
                //Close the connection
                try
                {
                    ((WarehouseAdapterEndPoint)transaction.EndPoint).Connection.Close();
                }
                catch (OracleException)
                {
                }
                catch (DbException)
                {
                }
                catch (InvalidOperationException)
                {
                }
                finally
                {
                    //Dispose the connection and destroy the endpoint
                    try
                    {
                        ((WarehouseAdapterEndPoint)transaction.EndPoint).Connection.Dispose();
                    }
                    catch (OracleException)
                    {
                    }
                    catch (DbException)
                    {
                    }
                    finally
                    {
                        OnEndPointDestroyed(transaction.EndPoint);
                    }
                }
            }
        }

        /// <summary>
        /// Callback method to abort a transaction that was started by this adapter.
        /// </summary>
        /// <param name="transaction">The transaction to abort.</param>
        public void Abort(AdapterTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            IDbTransaction dbTransaction = transaction.UnderlyingTransaction as IDbTransaction;
            
            try
            {
                //Rollback transaction
                dbTransaction.Rollback();
                transaction.State = TransactionState.Aborted;
            }
            catch (OracleException ex)
            {
                throw new AdapterException("The transaction could not be aborted.", ex);
            }
            catch (DbException ex)
            {
                throw new AdapterException("The transaction could not be aborted.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new AdapterException("The transaction could not be aborted.", ex);
            }
            finally
            {
                //Close the connection
                try
                {
                    ((WarehouseAdapterEndPoint)transaction.EndPoint).Connection.Close();
                }
                catch (OracleException)
                {
                }
                catch (DbException)
                {
                }
                catch (InvalidOperationException)
                {
                }
                finally
                {
                    //Dispose the connection and destroy the endpoint
                    try
                    {
                        ((WarehouseAdapterEndPoint)transaction.EndPoint).Connection.Dispose();
                    }
                    catch (OracleException)
                    {
                    }
                    catch (DbException)
                    {
                    }
                    finally
                    {
                        OnEndPointDestroyed(transaction.EndPoint);
                    }
                }
            }
        }

        /// <summary>
        /// Starts a transaction for this adapter
        /// </summary>
        /// <param name="msg">The message that is to be enlisted in the transaction</param>
        /// <returns>The transaction in which the message should be enlisted</returns>
        public AdapterTransaction StartTransaction(MultiPartMessage msg)
        {
            if (msg == null)
                throw new ArgumentNullException("msg");

            Uri sendUri = msg.Metadata.Read("SendUri") as Uri;

            //The warehouse instance is the host part of the send uri
            string instanceId = sendUri.Host;

            //Check if a transaction is already enlisted in the current scope for this warehouse instance
            AdapterTransaction transaction = TransactionScope.Current[instanceId];

            //Transaction not enlisted, create one
            if (transaction == null)
            {
                transaction = CreateTransaction(instanceId);
            }
            
            return transaction;
        }

        /// <summary>
        /// Transmits a message as part of a transaction
        /// </summary>
        /// <param name="msg">The message to transmit</param>
        /// <param name="transaction">The transaction in which the message is enlisted</param>
        /// <exception cref="WarehouseAdapterException">
        /// </exception>
        public void TransmitMessage(MultiPartMessage msg, AdapterTransaction transaction)
        {
            bool autoTransaction = false;

            try
            {
                //Auto create transaction if not defined
                if (transaction == null)
                {
                    autoTransaction = true;

                    Uri sendUri = msg.Metadata.Read("SendUri") as Uri;

                    //The warehouse instance is the host part of the uri
                    transaction = CreateTransaction(sendUri.Host);
                }

                MultiPartMessage responseMsg = null;
                IDbTransaction dbTransaction = (IDbTransaction)transaction.UnderlyingTransaction;

                using (DbCommandWrapper command = CreateDbCommand(msg))
                {
                    command.Command.Transaction = dbTransaction;
                    command.Command.Connection = dbTransaction.Connection;

                    responseMsg = ExecuteCommand(command, msg.MessageType);
                }

                if (responseMsg.Properties.Contains("ALMID_O"))
                {
                    if (responseMsg.Properties.Read("ALMID_O").GetType() != DBNull.Value.GetType())
                    {
                        string almId = responseMsg.Properties.ReadAsString("ALMID_O");
                        string nLangCod = "";

                        if (msg.Metadata.Contains("LanguageCode"))
                            nLangCod = msg.Metadata.ReadAsString("LanguageCode");

                        string alarmText = GetAlarmText(almId, nLangCod, dbTransaction.Connection, dbTransaction);

                        throw new WarehouseAdapterException(almId, alarmText);
                    }
                }

                if (autoTransaction && (transaction.State == TransactionState.Started))
                    Commit(transaction);

                responseMsg.Metadata.Write("CorrelationId", msg.MessageId);

                OnMessageReceived(responseMsg, transaction.EndPoint);
            }
            catch (Exception)
            {
                if ((transaction != null)
                    && (autoTransaction)
                    && (transaction.State == TransactionState.Started))
                {
                    Abort(transaction);
                }

                throw;
            }
        }

        /// <summary>
        /// Disposes any resources used by the adapter.
        /// </summary>
        /// <param name="disposing">True if called from user code, otherwise false.</param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                try
                {
                    if (disposing)
                    {
                        foreach (Timer timer in pollTimerCollection)
                        {
                            timer.Dispose();
                        }
                    }
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }

    }
}

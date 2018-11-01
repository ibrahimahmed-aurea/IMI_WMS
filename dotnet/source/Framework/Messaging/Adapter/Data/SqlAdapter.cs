using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Imi.MWFramework.Messaging;
using Imi.MWFramework.Adapter;

namespace Imi.MWFramework.Adapter.Data
{
    public class SqlAdapter : BaseAdapter
    {
        public SqlAdapter(Imi.MWFramework.Messaging.BasePropertyCollection configuration, string adapterId)
            : base(configuration, adapterId)
        { 
        
        }

        private DbProviderFactory GetProviderFactory()
        {
            return DbProviderFactories.GetFactory((string)Configuration.Read("Provider"));
        }

        private DbConnection CreateConnection(DbProviderFactory factory)
        {
            DbConnection connection = factory.CreateConnection();
            connection.ConnectionString = (string)Configuration.Read("ConnectionString");

            return connection;
        }
                
        public override void TransmitMessage(BaseMessage msg)
        {
            bool isQuery = false;
            bool ownsTransaction = false;
            bool ownsConnection = false;
            
            DbConnection connection;
            DbProviderFactory factory = GetProviderFactory();
            
            if (msg.Metadata.Contains("Connection"))
                connection = (DbConnection)msg.Metadata.Read("Connection");
            else
            {
                connection = CreateConnection(factory);
                ownsConnection = true;
            }

            DbCommand cmd = connection.CreateCommand();
            
            cmd.CommandText = (string)msg.Metadata.Read("CommandText");
            cmd.CommandType = (CommandType)msg.Metadata.Read("CommandType");
            cmd.Connection = connection;
                        
            if (msg.Metadata.Contains("IsQuery"))
                isQuery = (bool)msg.Metadata.Read("IsQuery");

            if (msg.Metadata.Contains("Transaction"))
            {
                cmd.Transaction = (DbTransaction)msg.Metadata.Read("Transaction");
            }
            else
            {
                cmd.Transaction = connection.BeginTransaction();
                ownsTransaction = true;
            }

            try
            {
                if (ownsConnection)
                    connection.Open();

                if (isQuery)
                {
                    DataSet ds = new DataSet();

                    DbDataAdapter dataAdapter = factory.CreateDataAdapter();
                    dataAdapter.SelectCommand = cmd;

                    dataAdapter.Fill(ds);
                }
                else
                {
                    cmd.ExecuteNonQuery();
                }

                if (ownsTransaction)
                    cmd.Transaction.Commit();
            }
            catch
            {
                if (ownsTransaction)
                    cmd.Transaction.Rollback();
            }
            finally
            {
                if (ownsConnection)
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
        }

        public override void Initialize()
        {
            
        }
                
        public override string ProtocolType
        {
            get 
            {
                return "sql";
            }
        }
                
    }
}

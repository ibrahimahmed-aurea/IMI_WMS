using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;

namespace Imi.Framework.DataAccess
{
    public class DbConnection : IDbConnection
    {
        private IDbConnection connection;
        private bool isScoped;
        private string connectionString;
                
        public DbConnection(string connectionString)
        {
            this.connectionString = connectionString;
        }
                        
        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return connection.BeginTransaction(il);
        }

        public IDbTransaction BeginTransaction()
        {
            return connection.BeginTransaction();
        }

        public void ChangeDatabase(string databaseName)
        {
            connection.ChangeDatabase(databaseName);
        }

        public void Close()
        {
            if (!isScoped)
                connection.Close();
        }

        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }

        public int ConnectionTimeout
        {
            get 
            {
                return connection.ConnectionTimeout;
            }
        }

        public IDbCommand CreateCommand()
        {
            return connection.CreateCommand();
        }

        public string Database
        {
            get 
            { 
                return connection.Database;
            }
        }

        public void Open()
        {
            if (TransactionScope.Current != null)
            {
                if (TransactionScope.Current.Transaction == null)
                {
                    connection = new OracleConnection(connectionString);
                    
                    try
                    {
                        connection.Open();
                        TransactionScope.Current.Transaction = connection.BeginTransaction();
                        
                    }
                    catch
                    {
                        connection.Close();
                        throw;
                    }
                }
                else
                    connection = TransactionScope.Current.Transaction.Connection;

                isScoped = true;
            }
            else
            {
                connection = new OracleConnection(connectionString);
                
                try
                {
                    connection.Open();
                }
                catch
                {
                    connection.Close();
                    throw;
                }
            }
            
        }

        public ConnectionState State
        {
            get
            { 
                return connection.State;
            }
        }
                
        public void Dispose()
        {
            if ((connection != null) && (!isScoped))
                connection.Dispose();
        }
    }
}

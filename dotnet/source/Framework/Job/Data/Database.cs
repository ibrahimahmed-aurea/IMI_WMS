#define ODP_NET

using System;
using System.Data;

#if ODP_NET
using Oracle.DataAccess.Client;
#else
using System.Data.OracleClient;
#endif

namespace Imi.Framework.Job.Data
{
    /// <summary>
    /// Summary description for OracleDatabase.
    /// </summary>
    public class Database : Object, IDbConnectionProvider
    {
        private IDbConnection _connection = null;
        private IDbTransaction _currentTransaction = null;

        public IDbTransaction CurrentTransaction
        {
            get { return _currentTransaction; }
            set { _currentTransaction = value; }
        }

        public Database(string connectionString)
        {
            _connection = new OracleConnection(connectionString);
            _connection.Open();
        }

        public bool IsConnected
        {
            get
            { 
                if (_connection != null)
                {
                    try
                    {
                        using (IDbCommand command = _connection.CreateCommand())
                        {
                            command.CommandText = "select * from dual";
                            
                            using (IDataReader reader = command.ExecuteReader())
                            {
                            }
                        }
                        
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }

                return false;
            }
        }

        public IDbConnection GetConnection()
        {
            return _connection;
        }

        public IDataReader ExecuteReader(string sqlText)
        {
            IDbCommand command = _connection.CreateCommand();
            command.CommandText = sqlText;
            command.Prepare();
            command.Transaction = _currentTransaction;
            return (command.ExecuteReader());
        }

        public IDbCommand CreateStoredProcedure(string procedureName)
        {
            IDbCommand command = _connection.CreateCommand();
            command.CommandText = procedureName;
            command.CommandType = System.Data.CommandType.StoredProcedure;
#if ODP_NET
            ((OracleCommand)command).BindByName = true;
#endif
            command.Transaction = _currentTransaction;

            return (command);
        }

        public void StartTransaction()
        {
            if (_currentTransaction == null)
            {
                _currentTransaction = _connection.BeginTransaction();
            }
        }

        public void Commit()
        {
            if (_currentTransaction != null)
            {
                try
                {
                    _currentTransaction.Commit();
                }
                finally
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void Rollback()
        {
            if (_currentTransaction != null)
            {
                try
                {
                    _currentTransaction.Rollback();
                }
                finally
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void Close()
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }

            if (_connection != null)
            {
                // Uncommitted transactions are rolled back by default.
                try
                {
                    _connection.Close();
                    _connection.Dispose();
                }
                catch (Exception) { } // ignore
                _connection = null;
            }
        }

        public void Dispose()
        {
            Close();
        }
    }


}


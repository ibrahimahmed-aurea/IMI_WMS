#define ODP_NET
using System;
using System.Data;
using System.Data.Common;
#if ODP_NET
using Oracle.DataAccess.Client;
#else
using System.Data.OracleClient;
#endif

namespace Imi.CodeGenerators.WebServices.Framework
{
    /// <summary>
    /// Summary description for OracleDatabase.
    /// </summary>
    [Obsolete("use Imi.Framework.Job.Data.Database")]
    public class Database
    {
        private static IDbConnection OWDB = null;
        private static Database me = null;
        private static IDbTransaction transaction = null;

        public Database(String connectionString)
        {
            OWDB = new OracleConnection(connectionString);
            OWDB.Open();
            me = this;
        }

        public static IDbConnection GetDbConnection()
        {
            return OWDB;
        }

        public static IDbTransaction GetCurrentTransaction()
        {
            return (transaction);
        }

        public IDataReader ExecuteReader(String sqlText)
        {
            IDbCommand aQuery = OWDB.CreateCommand();
            aQuery.CommandText = sqlText;
            aQuery.Prepare();

            return (aQuery.ExecuteReader());
        }

        public IDbCommand CreateStoredProcedure(String procedureName)
        {
            IDbCommand sp = OWDB.CreateCommand(); ;
            sp.CommandText = procedureName;
            sp.CommandType = System.Data.CommandType.StoredProcedure;
            sp.Connection = OWDB;
#if ODP_NET
            ((OracleCommand)sp).BindByName = true;
#endif
            return (sp);
        }

        public void StartTransaction()
        {
            if (transaction == null)
            {
                transaction = OWDB.BeginTransaction();
            }
        }

        public void Commit()
        {
            if (transaction != null)
            {
                try
                {
                    transaction.Commit();
                }
                finally
                {
                    transaction.Dispose();
                    transaction = null;
                }
            }
        }

        public void Rollback()
        {
            if (transaction != null)
            {
                try
                {
                    transaction.Rollback();
                }
                finally
                {
                    transaction.Dispose();
                    transaction = null;
                }
            }
        }

        public void Close()
        {
            if (transaction != null)
            {
                transaction.Dispose();
                transaction = null;
            }

            if (OWDB != null)
            {
                // Uncommitted transactions are rolled back by default.
                try
                {
                    OWDB.Close();
                    OWDB.Dispose();
                }
                catch (Exception) { } // ignore
                OWDB = null;
            }
        }

        public void Dispose()
        {
            Close();
        }

    }


}


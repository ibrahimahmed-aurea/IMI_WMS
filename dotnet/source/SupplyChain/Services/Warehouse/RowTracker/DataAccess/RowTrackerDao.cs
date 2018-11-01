using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using System.Data;
using Oracle.DataAccess.Client;
using Imi.SupplyChain.Warehouse.RowTracker.BusinessEntities;

namespace Imi.SupplyChain.Warehouse.RowTracker.DataAccess
{
    public class RowTrackerDao : DataAccessObject, IRowTrackerDao
    {
        public RowTrackerDao(string connectionString)
            : base(connectionString)
        { 
        }

        public FindRowIdentityResult FindRowIdentity(FindRowIdentityParameters parameters)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                FindRowIdentityResult result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "RowTracker.Find";
                        ((OracleCommand)command).BindByName = true;

                        foreach (IDbDataParameter parameter in FindRowIdentityTranslator.TranslateParameters(parameters))
                            command.Parameters.Add(parameter);

                        command.Prepare();
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();
                        result = FindRowIdentityTranslator.TranslateResult(command.Parameters);
                    }
                }

                scope.Complete();

                return result;
            }
        }

        public void EnableTracking()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "RowTracker.Enable";
                        ((OracleCommand)command).BindByName = true;
                        command.Prepare();
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();
                    }
                }

                scope.Complete();
            }
        }

        public void SetIsLastMultiSelectRow(bool isLastMultiSelectRow)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        string paramValue = "0";

                        if (isLastMultiSelectRow)
                            paramValue = "1";

                        command.CommandText = "RowTracker.SetIsLastMultiSelectRow";
                        ((OracleCommand)command).BindByName = true;
                        command.Parameters.Add(new OracleParameter("IsLastMultiSelectRow_I", OracleDbType.Varchar2, paramValue, ParameterDirection.Input));
                        command.Prepare();
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();
                    }
                }

                scope.Complete();
            }
        }
    }
}

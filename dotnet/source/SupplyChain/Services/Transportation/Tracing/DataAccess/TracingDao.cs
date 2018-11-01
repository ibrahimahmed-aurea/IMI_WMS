using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using System.Data;
using Oracle.DataAccess.Client;
using Imi.SupplyChain.Transportation.Tracing.BusinessEntities;
using Microsoft.Practices.Unity.InterceptionExtension;
using Imi.SupplyChain.Transportation.Tracing.DataAccess.Translators;

namespace Imi.SupplyChain.Transportation.Tracing.DataAccess
{
    [Tag("RMUSER")]
    public class TracingDao : DataAccessObject, ITracingDao
    {
        public TracingDao(string connectionString)
            : base(connectionString)
        { 
        }

        public StartDatabaseTracingResult StartDatabaseTracing(StartDatabaseTracingParameters parameters)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                StartDatabaseTracingResult result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "Delphi.Start_Log";
                        ((OracleCommand)command).BindByName = true;

                        foreach (IDbDataParameter parameter in StartDatabaseTracingTranslator.TranslateParameters(parameters))
                            command.Parameters.Add(parameter);

                        command.Prepare();
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();
                        result = new StartDatabaseTracingResult();
                    }
                }

                scope.Complete();

                return result;
            }
        }

        public StopDatabaseTracingResult StopDatabaseTracing(StopDatabaseTracingParameters parameters)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                StopDatabaseTracingResult result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "Delphi.Stop_Log";
                        ((OracleCommand)command).BindByName = true;

                        foreach (IDbDataParameter parameter in StopDatabaseTracingTranslator.TranslateParameters(parameters))
                            command.Parameters.Add(parameter);

                        command.Prepare();
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();
                        result = new StopDatabaseTracingResult();
                    }
                }

                scope.Complete();

                return result;
            }
        }

        public ModifyInterfaceTracingResult ModifyInterfaceTracing(ModifyInterfaceTracingParameters parameters)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ModifyInterfaceTracingResult result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SYSTEMCONFIG.MODIFY_LOGG";
                        ((OracleCommand)command).BindByName = true;

                        foreach (IDbDataParameter parameter in ModifyInterfaceTracingTranslator.TranslateParameters(parameters))
                            command.Parameters.Add(parameter);

                        command.Prepare();
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();
                        result = new ModifyInterfaceTracingResult();
                    }
                }

                scope.Complete();

                return result;
            }
        }


        public IList<CheckInterfaceTracingResult> CheckInterfaceTracing(CheckInterfaceTracingParameters parameters)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                IList<CheckInterfaceTracingResult> result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.Transportation.Tracing.DataAccess.Queries.CheckLogg.sql");
                        ((OracleCommand)command).BindByName = true;

                        foreach (IDbDataParameter parameter in CheckInterfaceTracingTranslator.TranslateParameters(parameters))
                            command.Parameters.Add(parameter);

                        LogDbCommand(command);

                        command.Prepare();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            result = CheckInterfaceTracingTranslator.TranslateResultSet(reader);
                        }
                    }
                }

                scope.Complete();

                return result;
            }
        }

        public IList<GetServerInformationResult> GetServerInformation(GetServerInformationParameters parameters)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                IList<GetServerInformationResult> result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.Transportation.Tracing.DataAccess.Queries.GetServerInformation.sql");
                        ((OracleCommand)command).BindByName = true;

                        foreach (IDbDataParameter parameter in GetServerInformationTranslator.TranslateParameters(parameters))
                            command.Parameters.Add(parameter);

                        LogDbCommand(command);

                        command.Prepare();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            result = GetServerInformationTranslator.TranslateResultSet(reader);
                        }
                    }
                }

                scope.Complete();

                return result;
            }
        }

    }
}

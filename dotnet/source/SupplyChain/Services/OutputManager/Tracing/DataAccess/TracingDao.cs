using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using System.Data;
using Oracle.DataAccess.Client;
using Imi.SupplyChain.OutputManager.Tracing.BusinessEntities;
using Microsoft.Practices.Unity.InterceptionExtension;
using Imi.SupplyChain.OutputManager.Tracing.DataAccess.Translators;

namespace Imi.SupplyChain.OutputManager.Tracing.DataAccess
{
    [Tag("OMUSER")]
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
                        command.CommandText = "Logg_Output.Start_Log_from_GUI";
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
                        command.CommandText = "Logg_Output.Stop_Log_from_GUI";
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
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.OutputManager.Tracing.DataAccess.Queries.GetServerInformation.sql");
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

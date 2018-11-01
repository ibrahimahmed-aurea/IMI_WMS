using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using System.Data;
using Oracle.DataAccess.Client;
using Imi.SupplyChain.Transportation.Authentication.BusinessEntities;

namespace Imi.SupplyChain.Transportation.Authentication.DataAccess
{
    public class AuthenticationDao : DataAccessObject, IAuthenticationDao
    {
        public AuthenticationDao(string connectionString)
            : base(connectionString)
        { 
        }

        public IList<FindUserNodesResult> FindAllNodes()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                IList<FindUserNodesResult> result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.Transportation.Authentication.DataAccess.Queries.FindAllNodes.sql");
                        command.Prepare();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            result = FindUserNodesTranslator.TranslateResultSet(reader);
                        }
                    }
                }

                scope.Complete();

                return result;
            }
        }

        public IList<FindUserNodesResult> FindUserNodes(FindUserNodesParameters parameters)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                IList<FindUserNodesResult> result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.Transportation.Authentication.DataAccess.Queries.FindUserNodes.sql");
                        ((OracleCommand)command).BindByName = true;

                        foreach (IDbDataParameter parameter in FindUserNodesTranslator.TranslateParameters(parameters))
                            command.Parameters.Add(parameter);

                        command.Prepare();
                                                
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            result = FindUserNodesTranslator.TranslateResultSet(reader);
                        }
                    }
                }

                scope.Complete();

                return result;
            }
        }

        public LogonResult Logon(LogonParameters parameters)
        {
            LogonResult result = null;

            using (TransactionScope scope = new TransactionScope())
            {
                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "Authentication.Logon_SmartClient";

                        ((OracleCommand)command).BindByName = true;

                        foreach (IDbDataParameter parameter in LogonTranslator.TranslateParameters(parameters))
                            command.Parameters.Add(parameter);

                        LogDbCommand(command);

                        command.Prepare();
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();
                        result = LogonTranslator.TranslateResult(command.Parameters);
                    }
                }

                scope.Complete();
            }

            return result;
        }

        public IList<FindUserLogonDetailsResult> FindUserLogonDetails(FindUserLogonDetailsParameters parameters)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                IList<FindUserLogonDetailsResult> result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.Transportation.Authentication.DataAccess.Queries.FindUserLogonDetails.sql");
                                                                                          
                        ((OracleCommand)command).BindByName = true;

                        foreach (IDbDataParameter parameter in FindUserLogonDetailsTranslator.TranslateParameters(parameters))
                            command.Parameters.Add(parameter);

                        command.Prepare();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            result = FindUserLogonDetailsTranslator.TranslateResultSet(reader);
                        }
                    }
                }

                scope.Complete();

                return result;
            }
        }

        public void ModifyUserDetails(ModifyUserDetailsParameters parameters)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.Transportation.Authentication.DataAccess.Queries.ModifyUserDetails.sql");
                        ((OracleCommand)command).BindByName = true;

                        foreach (IDbDataParameter parameter in ModifyUserDetailsTranslator.TranslateParameters(parameters))
                            command.Parameters.Add(parameter);

                        command.Prepare();

                        command.ExecuteNonQuery();
                    }
                }

                scope.Complete();
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using System.Data;
using Oracle.DataAccess.Client;
using Imi.SupplyChain.Warehouse.Authentication.BusinessEntities;

namespace Imi.SupplyChain.Warehouse.Authentication.DataAccess
{
    public class AuthenticationDao : DataAccessObject, IAuthenticationDao
    {
        public AuthenticationDao(string connectionString)
            : base(connectionString)
        { 
        }

        public IList<FindUserWarehousesResult> FindAllWarehouses()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                IList<FindUserWarehousesResult> result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.Warehouse.Authentication.DataAccess.Queries.FindAllWarehouses.sql");
                        command.Prepare();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            result = FindUserWarehousesTranslator.TranslateResultSet(reader);
                        }
                    }
                }

                scope.Complete();

                return result;
            }
        }

        public IList<FindUserCompaniesResult> FindAllCompanies()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                IList<FindUserCompaniesResult> result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.Warehouse.Authentication.DataAccess.Queries.FindAllCompanies.sql");
                        command.Prepare();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            result = FindUserCompaniesTranslator.TranslateResultSet(reader);
                        }
                    }
                }

                scope.Complete();

                return result;
            }
        }

        public IList<FindUserWarehousesResult> FindUserWarehouses(FindUserWarehousesParameters parameters)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                IList<FindUserWarehousesResult> result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.Warehouse.Authentication.DataAccess.Queries.FindUserWarehouses.sql");
                        ((OracleCommand)command).BindByName = true;

                        foreach (IDbDataParameter parameter in FindUserWarehousesTranslator.TranslateParameters(parameters))
                            command.Parameters.Add(parameter);

                        command.Prepare();
                                                
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            result = FindUserWarehousesTranslator.TranslateResultSet(reader);
                        }
                    }
                }

                scope.Complete();

                return result;
            }
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
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.Warehouse.Authentication.DataAccess.Queries.FindUserLogonDetails.sql");
                                                                                          
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
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.Warehouse.Authentication.DataAccess.Queries.ModifyUserDetails.sql");
                        ((OracleCommand)command).BindByName = true;

                        foreach (IDbDataParameter parameter in ModifyUserDetailsTranslator.TranslateParameters(parameters))
                        {
                            if(parameter.ParameterName != "WHID")
                                command.Parameters.Add(parameter);
                        }

                        command.Prepare();

                        command.ExecuteNonQuery();
                    }

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.Warehouse.Authentication.DataAccess.Queries.ModifyUserWarehouseDetails.sql");
                        ((OracleCommand)command).BindByName = true;

                        foreach (IDbDataParameter parameter in ModifyUserDetailsTranslator.TranslateParameters(parameters))
                        {
                            if ((parameter.ParameterName != "LASTLOGONDTM") && (parameter.ParameterName != "COMPANY_ID"))
                                command.Parameters.Add(parameter);
                        }

                        command.ExecuteNonQuery();
                    }
                }

                scope.Complete();
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
        
        public IList<FindUserCompaniesResult> FindUserCompanies(FindUserCompaniesParameters parameters)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                IList<FindUserCompaniesResult> result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.Warehouse.Authentication.DataAccess.Queries.FindUserCompanies.sql");
                        ((OracleCommand)command).BindByName = true;

                        foreach (IDbDataParameter parameter in FindUserCompaniesTranslator.TranslateParameters(parameters))
                            command.Parameters.Add(parameter);

                        command.Prepare();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            result = FindUserCompaniesTranslator.TranslateResultSet(reader);
                        }
                    }
                }

                scope.Complete();

                return result;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using System.Data;
using Oracle.DataAccess.Client;

namespace Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess
{
    internal class ConfigDao_Oracle : DataAccessObject, IConfigDao
    {

        public ConfigDao_Oracle(string connectionString)
			: base(connectionString)
		{
		}

        #region IConfigDao Members

        public IDictionary<string, Terminal> FindTerminal()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                IDictionary<string, Terminal> result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess.Queries.FindTerminal.sql");
                        ((OracleCommand)command).BindByName = true;

                        //LogDbCommand(command);

                        command.Prepare();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            result = FindTerminalTranslator.TranslateResultSet(reader);
                        }
                    }
                }

                scope.Complete();

                return result;
            }
        }

        public IList<DocumentType> FindDocumentType()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                IList<DocumentType> result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess.Queries.FindDocumentType.sql");
                        ((OracleCommand)command).BindByName = true;

                        //LogDbCommand(command);

                        command.Prepare();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            result = FindDocumentTypeTranslator.TranslateResultSet(reader);
                        }
                    }
                }

                scope.Complete();

                return result;
            }
        }

        public IDictionary<string, Report> FindReport()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                IDictionary<string, Report> result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess.Queries.FindReport.sql");
                        ((OracleCommand)command).BindByName = true;

                        //LogDbCommand(command);

                        command.Prepare();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            result = FindReportTranslator.TranslateResultSet(reader);
                        }
                    }
                }

                scope.Complete();

                return result;
            }
        }

        public IDictionary<string, Printer> FindPrinter(string outputManagerID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                IDictionary<string, Printer> result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess.Queries.FindPrinter.sql");
                        ((OracleCommand)command).BindByName = true;

                        FindPrinterTranslator.TranslateParameters(command.Parameters, outputManagerID);

                        //LogDbCommand(command);

                        command.Prepare();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            result = FindPrinterTranslator.TranslateResultSet(reader);
                        }
                    }
                }

                scope.Complete();

                return result;
            }
        }

        public IList<PrinterAssociation> FindPrinterAssociation(string outputManagerID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                IList<PrinterAssociation> result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess.Queries.FindPrinterAssociation.sql");
                        ((OracleCommand)command).BindByName = true;

                        FindPrinterAssociationTranslator.TranslateParameters(command.Parameters, outputManagerID);

                        //LogDbCommand(command);

                        command.Prepare();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            result = FindPrinterAssociationTranslator.TranslateResultSet(reader);
                        }
                    }
                }

                scope.Complete();

                return result;
            }
        }

        public IDictionary<string, Adapter> FindAdapter(string outputManagerID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                IDictionary<string, Adapter> result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess.Queries.FindAdapter.sql");
                        ((OracleCommand)command).BindByName = true;

                        FindAdapterTranslator.TranslateParameters(command.Parameters, outputManagerID);

                        //LogDbCommand(command);

                        command.Prepare();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            result = FindAdapterTranslator.TranslateResultSet(reader);
                        }
                    }
                }

                scope.Complete();

                return result;
            }
        }

        public DateTime? FindConfigUpdateTime(string outputManagerID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                DateTime? result = null;

                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = StatementCache.Instance.GetCachedStatement("Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess.Queries.FindUpdateTime.sql");
                        ((OracleCommand)command).BindByName = true;

                        IDataParameter dbParameter = new OracleParameter();
                        dbParameter.ParameterName = "OMID";
                        dbParameter.DbType = DbTypeConvertor.ConvertToDbType(typeof(string));
                        dbParameter.Direction = ParameterDirection.Input;
                        dbParameter.Value = outputManagerID;

                        command.Parameters.Add(dbParameter);

                        //LogDbCommand(command);

                        command.Prepare();

                        object upddtm = command.ExecuteScalar();

                        if (upddtm != null && upddtm != DBNull.Value)
                        {
                            result = DbTypeConvertor.Convert<DateTime>(upddtm);
                        }
                    }
                }

                scope.Complete();

                return result;
            }
        }

        public void LoggError(string outputManagerID, string errorMessage)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "OutputManager.New_Error";
                        ((OracleCommand)command).BindByName = true;

                         LoggErrorTranslator.TranslateParameters(command.Parameters, outputManagerID, errorMessage);

                        //LogDbCommand(command);

                        command.Prepare();
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();
                    }
                }

                scope.Complete();
            }
        }

        public void UpdateOM_URL(string outputManagerID, bool mainService, string URL)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (IDbConnection connection = new DbConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "OutputManager.Modify_URL";
                        ((OracleCommand)command).BindByName = true;

                        UpdateOM_URL_Translator.TranslateParameters(command.Parameters, outputManagerID, mainService, URL);

                        //LogDbCommand(command);

                        command.Prepare();
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();
                    }
                }

                scope.Complete();
            }
        }

        #endregion
    }
}
